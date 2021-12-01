using ASM.Core.Models;
using ASM.Core.Services;
using ASM.Imp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IStorageService storageService;
        private readonly IMeliService meliService;

        public NotificationController(IStorageService storageService, IMeliService meliService)
        {
            this.storageService = storageService;
            this.meliService = meliService;
        }

        [HttpPost("NotificationTrigger")]
        public async Task NotificationTrigger([FromBody]NotificationTrigger notification)
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
