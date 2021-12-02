using ASM.Core.Models;
using ASM.Core.Repositories;
using ASM.Core.Services;
using ASM.Data.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
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
        public async Task Run([QueueTrigger("process-order-notification")]NotificationTrigger notification, ILogger log)
        {
            /*
             TODO - AzFunction
                1 - verify if this is the first message
                2 - Get message by sellerId
                3 - Build string message 
                4 - Send message
                5 - check as first message
             */

            var order = await meliService.GetOrderDetailsAsync(notification);
            if (order.Success)
            {
                await meliService.SendMessageToBuyerAsync(new SendMessage
                {
                    BuyerId = order.buyer.id,
                    Message = "Test",
                    SellerId = order.seller.id,
                    PackId = notification.OrderId
                });
                log.LogInformation($"message sent successfully to BuyerId:{order.buyer.id}");
            }
            else
            {
                log.LogError($"message has not sent to BuyerId:{order.buyer.id}");
            }
            
        }
    }
}
