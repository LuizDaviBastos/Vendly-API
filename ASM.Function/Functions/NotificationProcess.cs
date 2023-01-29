using ASM.Data.Interfaces;
using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using System.Threading.Tasks;

namespace ASM.Core.Function.Functions
{
    public class NotificationProcess
    {
        private readonly IUnitOfWork uow;
        private readonly IMeliService meliService;
        public NotificationProcess(IUnitOfWork uow, IMeliService meliService)
        {
            this.uow = uow;
            this.meliService = meliService;
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
            var enabled = uow.SellerRepository.GetBySellerId(notification.user_id)?.AfterSellerMessageEnabled ?? true;
            if (!enabled) return;

            //TODO - https://developers.mercadolivre.com.br/pt_br/aplicativos#Usu%C3%A1rios-que-outorgaram-licen%C3%A7as-a-seu-aplicativo
            if (!notification.OrderIdIsValid)
            {
                var message = $"Error to get OrderId (OrderId is:{notification.OrderId})";
                log.LogError(message);
                throw new System.Exception(message);
            }

            var sendMessage = new SendMessage
            {
                SellerId = notification.user_id,
                PackId = notification.OrderId //important to send message to buyer
            };

            var isFirstSellerMessage = await meliService.IsFirstSellerMessage(sendMessage);
            if (isFirstSellerMessage)
            {
                var order = await meliService.GetOrderDetailsAsync(notification);
                if (order.Success ?? false)
                {
                    sendMessage.BuyerId = order.buyer.id;

                    //Get message by seller
                    var seller = uow.SellerRepository.GetBySellerId(notification.user_id);

                    if (!string.IsNullOrEmpty(seller?.Message))
                    {
                        //Prepare sellerMessage
                        sendMessage.Message = Utils.PrepareSellerMessage(seller.Message, order);
                        await meliService.SendMessageToBuyerAsync(sendMessage);
                        log.LogInformation($"message sent successfully to BuyerId:{order.buyer.id}");
                    }
                    else
                    {
                        log.LogInformation($"Message not defined sellerId: {seller?.id}");
                    }
                }
            }
        }

        
    }
}
