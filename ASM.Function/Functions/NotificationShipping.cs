using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Function.Services;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ASM.Core.Function.Functions
{
    public class NotificationShipping
    {
        private readonly ISellerService sellerService;
        private readonly SendMessageService sendMessageService;

        public NotificationShipping(ISellerService sellerService, SendMessageService sendMessageService)
        {
            this.sellerService = sellerService;
            this.sendMessageService = sendMessageService;
        }

        [FunctionName("NotificationShipping")]
        public async Task Run([QueueTrigger("process-shipping-notification")] NotificationTrigger notification, ILogger log)
        {
            
            var sellerMessage = await sellerService.GetMessageByMeliSellerId(notification.user_id, MessageType.Shipping);
            SellerOrder sellerOrder = await sellerService.GetSellerOrder(sellerMessage.MeliAccount.SellerId.Value, notification.user_id, notification.TopicId, MessageType.Shipping);
            if (!(sellerMessage?.Activated ?? false) || sellerOrder.ShippingMessageStatus == true) return;

            if (!notification.TopicIdIsValid)
            {
                var message = $"Error to get OrderId (OrderId is: {notification.TopicId})";
                log.LogError(message);
                throw new System.Exception(message);
            }

            await sendMessageService.Send(notification, sellerMessage, MessageType.Shipping, log);
        }
    }
}
