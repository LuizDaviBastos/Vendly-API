using ASM.Services.Interfaces;
using ASM.Services.Models;
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
        public async Task<IActionResult> NotificationTrigger([FromBody]NotificationTrigger notification)
        {
            if (notification.IsOrderV2)
            {
                await storageService.SendMessageAsync("process-order-notification", notification);
            }
            return Ok();
        }

        //ngrok http -host-header=localhost https://localhost:7024/
    }
}
