using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ASM.Core.Function.Functions
{
    public class NotificationOrder
    {
        private readonly IUnitOfWork uow;
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;

        public NotificationOrder(IUnitOfWork uow, IMeliService meliService, ISellerService sellerService)
        {
            this.uow = uow;
            this.meliService = meliService;
            this.sellerService = sellerService;
        }

        [FunctionName("NotificationProcess")]
        public async Task Run([QueueTrigger("process-order-notification")] NotificationTrigger notification, ILogger log)
        {
            /*
             TODO - AzFunction
                1 - Build string message 
                2 - dont send if not have payment (todo)
                3- make routine to enable/disable flag of payment
             */

            //Can send after seller messages
            
            var sellerMessage = await sellerService.GetMessageByMeliSellerId(notification.user_id, MessageType.AfterSeller);
            Guid sellerId = sellerMessage.MeliAccount.SellerId.Value;
            SellerOrder sellerOrder = await sellerService.GetSellerOrder(sellerId, notification.TopicId, MessageType.AfterSeller);
            
            if (!(sellerMessage?.Activated ?? false) || sellerOrder.AfterSellerMessageStatus == true) return;

            //TODO - https://developers.mercadolivre.com.br/pt_br/aplicativos#Usu%C3%A1rios-que-outorgaram-licen%C3%A7as-a-seu-aplicativo
            if (!notification.TopicIdIsValid)
            {
                var message = $"Error to get OrderId (OrderId is: {notification.TopicId})";
                log.LogError(message);
                throw new System.Exception(message);
            }


            var sendMessage = new SendMessage
            {
                MeliSellerId = notification.user_id,
                PackId = notification.TopicId, //important to send message to buyer
                Message = sellerMessage.Message
            };

            var order = await meliService.GetOrderDetailsAsync(notification);
            if (order.Success ?? false)
            {
                sendMessage.BuyerId = order.buyer.id;
                if (!string.IsNullOrEmpty(sellerMessage?.Message))
                {
                    var result = await meliService.SaveAttachments(sellerId, sellerMessage);
                    if (result.Any()) sendMessage.Attachments = result.Select(x => x.Id).ToList();

                    //Prepare sellerMessage
                    sendMessage.Message = Utils.PrepareSellerMessage(sellerMessage.Message, order);
                    await meliService.SendMessageToBuyerAsync(sendMessage);
                    await sellerService.SaveOrUpdateOrderMessageStatus(sellerId, notification.TopicId, MessageType.AfterSeller, true);
                    log.LogInformation($"message sent successfully to BuyerId: {order.buyer.id} | type: AfterSeller");
                }
                else
                {
                    log.LogInformation($"Message not defined MeliAccountId: {sellerMessage?.MeliAccountId}");
                }
            } 
            else {
                log.LogInformation($"Error to get order details. MeliAccountId: {sellerMessage?.MeliAccountId}");
            }
        }

        
    }
}
