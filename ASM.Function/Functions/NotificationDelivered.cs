using ASM.Data.Enums;
using ASM.Function.Services;
using ASM.Services;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ASM.Core.Function.Functions
{
    public class NotificationDelivered
    {
        private readonly ISellerService sellerService;
        private readonly SendMessageService sendMessageService;
        private readonly PaymentService paymentService;

        public NotificationDelivered(ISellerService sellerService, SendMessageService sendMessageService, PaymentService paymentService)
        {
            this.sellerService = sellerService;
            this.sendMessageService = sendMessageService;
            this.paymentService = paymentService;
        }

        [FunctionName("NotificationDelivered")]
        public async Task Run([QueueTrigger("process-delivered-notification")] NotificationTrigger notification, ILogger log)
        {
            var status = await paymentService.ExpirateDateValid(notification.user_id);
            if (!status.NotExpired) return;

            var sellerMessage = await sellerService.GetMessageByMeliSellerId(notification.user_id, MessageType.Delivered);
            var sellerOrder = await sellerService.GetSellerOrder(sellerMessage.MeliAccount.SellerId.Value, notification.user_id, notification.TopicId, MessageType.Delivered);
            if (!(sellerMessage?.Activated ?? false) || sellerOrder.DeliveredMessageStatus == true) return;

            if (!notification.TopicIdIsValid)
            {
                var message = $"Error to get OrderId (OrderId is: {notification.TopicId})";
                log.LogError(message);
                throw new System.Exception(message);
            }

            await sendMessageService.Send(notification, sellerMessage, MessageType.Delivered, log);
        }
    }
}
