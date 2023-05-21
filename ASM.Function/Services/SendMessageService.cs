using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Services.Helpers;
using ASM.Services;
using ASM.Services.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Servers;
using System;
using System.Threading.Tasks;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using Microsoft.Azure.Documents.Spatial;
using System.Runtime.ConstrainedExecution;
using System.Linq;

namespace ASM.Function.Services
{
    public class SendMessageService
    {
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;

        public SendMessageService(IMeliService meliService, ISellerService sellerService)
        {
            this.meliService = meliService;
            this.sellerService = sellerService;
        }

        public async Task Send(NotificationTrigger notification, SellerMessage sellerMessage, MessageType messageType, ILogger log)
        {
            Guid sellerId = sellerMessage.MeliAccount.SellerId.Value;
            string enumName = Enum.GetName(typeof(MessageType), messageType);
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
                    (bool status, string message) = await meliService.SendMessageToBuyerAsync(sendMessage);
                    if (status)
                    {
                        await sellerService.SaveOrUpdateOrderMessageStatus(sellerId, notification.user_id, notification.TopicId, messageType, true);
                        log.LogInformation($"message sent successfully to Buyer. MeliAccountId: {notification.user_id} | type: {enumName}");
                    }
                    else
                    {
                        log.LogError($"Error to send message to Buyer. MeliAccountId: {notification.user_id} | type: {enumName}");
                        throw new Exception(message);
                    }
                }
                else
                {
                    log.LogInformation($"Message not defined MeliAccountId: {sellerMessage?.MeliAccountId}");
                }
            }
            else
            {
                log.LogInformation($"Error to get order details. MeliAccountId: {sellerMessage?.MeliAccountId}");
            }
        }
    }
}
