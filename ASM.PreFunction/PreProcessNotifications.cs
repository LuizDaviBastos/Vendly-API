using System;
using System.Threading.Tasks;
using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Services;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ASM.PreFunction
{
    public class PreProcessNotifications
    {
        private readonly IStorageService storageService;
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;

        public PreProcessNotifications(IStorageService storageService, IMeliService meliService, ISellerService sellerService)
        {
            this.storageService = storageService;
            this.meliService = meliService;
            this.sellerService = sellerService;
        }

        [FunctionName("PreProcessNotifications")]
        public async Task Run([QueueTrigger("pre-process-notifications")] NotificationTrigger notification, ILogger log)
        {
            if(notification.IsOrderV2)
            {
                var sellerOrder = await sellerService.GetSellerOrder(notification.user_id, notification.TopicId, MessageType.AfterSeller);
                if (sellerOrder?.AfterSellerMessageStatus == true) return;

                await storageService.SendMessageAsync("process-order-notification", notification);
            }
            else if (notification.IsShipping)
            {
                var shipping = await meliService.GetShipmentDetails(notification);
                bool sent = (shipping.status == "ready_to_ship" && shipping.substatus == "in_hub") || (shipping.status == "shipped");
                bool delivered = shipping.status == "delivered";
                if (sent)
                {
                    await storageService.SendMessageAsync("process-shipping-notification", notification);
                }
                else if (delivered)
                {
                    await storageService.SendMessageAsync("process-delivered-notification", notification);
                }
            }
            else if (notification.IsFeedback)
            {
                var feedback = await meliService.GetFeedbackDetailsAsync(notification);
                if (feedback?.purchase?.fulfilled ?? false)
                {
                    await storageService.SendMessageAsync("process-delivered-notification", notification);
                }
            }
        }
    }
}
