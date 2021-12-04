using ASM.Core.Repositories;
using ASM.Data.Entities;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Core.Function
{
    public class NotificationProcess
    {
        private readonly IRepository<Seller> sellerRepository;
        private readonly IMeliService meliService;
        public NotificationProcess(IRepository<Seller> sellerRepository, IMeliService meliService)
        {
            this.sellerRepository = sellerRepository;
            this.meliService = meliService;
        }

        [FunctionName("NotificationProcess")]
        public async Task Run([QueueTrigger("process-order-notification")] NotificationTrigger notification, ILogger log)
        {
            /*
             TODO - AzFunction
                1 - verify if this is the first message - x
                2 - Get message by sellerId - x
                3 - Build string message 
                4 - Send message - x
                5 - check as first message - x
             */

            //TODO - https://developers.mercadolivre.com.br/pt_br/aplicativos#Usu%C3%A1rios-que-outorgaram-licen%C3%A7as-a-seu-aplicativo

            var sendMessage = new SendMessage
            {
                SellerId = notification.user_id,
                PackId = notification.OrderId
            };

            var isFirstSellerMessage = await meliService.IsFirstSellerMessage(sendMessage);
            if (isFirstSellerMessage)
            {
                var order = await meliService.GetOrderDetailsAsync(notification);
                if (order.Success)
                {
                    sendMessage.BuyerId = order.buyer.id;
                    sendMessage.Message = "Message not defined";

                    var sellerMessage = sellerRepository.GetQueryable(x => x.SellerId == notification.user_id)
                        .Select(x => x.Message)
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(sellerMessage))
                    {
                        //TODO build sellerMessage
                        sendMessage.Message = sellerMessage;
                    }

                    await meliService.SendMessageToBuyerAsync(sendMessage);

                    log.LogInformation($"message sent successfully to BuyerId:{order.buyer.id}");
                }
            }
        }
    }
}
