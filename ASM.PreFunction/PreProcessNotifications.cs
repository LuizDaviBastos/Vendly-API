using System;
using System.Threading.Tasks;
using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Services;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using ASM.Services.Models.Constants;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                notification.OrderId = notification.TopicId.ToString();
                var sellerOrder = await sellerService.GetSellerOrder(notification.user_id, notification.TopicId, MessageType.AfterSeller);
                if (sellerOrder?.AfterSellerMessageStatus == true) return;

                await storageService.SendMessageAsync("process-order-notification", notification);
            }
            else if (notification.IsShipping)
            {
                var shipping = await meliService.GetShipmentDetails(notification);
                bool sent = (shipping.status == ShipmentStatus.ReadyToShip && shipping.substatus == ShipmentSubStatus.InHub) || (shipping.status == ShipmentStatus.Shipped);
                bool delivered = shipping.status == ShipmentStatus.Delivered;
                if (sent)
                {
                    notification.OrderId = shipping.order_id;
                    notification.trackingNumber = shipping.tracking_number;
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
                //notification.OrderId = feedback.order_id;
                if (feedback?.purchase?.fulfilled ?? false)
                {
                    notification.OrderId = feedback.purchase.order_id.ToString();
                    await storageService.SendMessageAsync("process-delivered-notification", notification);
                }
            }
        }
    }
}
