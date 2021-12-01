using ASM.Core.Models;
using ASM.Core.Services;
using ASM.Imp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class NotificationTriggerController : ControllerBase
    {
        private readonly IStorageService storageService;
        private readonly IMeliService meliService;

        public NotificationTriggerController(IStorageService storageService, IMeliService meliService)
        {
            this.storageService = storageService;
            this.meliService = meliService;
        }

        [HttpPost("NotificationTrigger")]
        public async Task NotificationTrigger(NotificationTrigger notification)
        {
            if (notification.IsOrderV2)
            {
                var result = await meliService.GetOrderDetailsAsync(notification);
                await storageService.SendMessageAsync("process-order-notification", notification);
            }   
        }

        //ngrok http -host-header=localhost https://localhost:7024/
    }
}
