using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IStorageService storageService;

        public NotificationController(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        [HttpPost("NotificationTrigger")]
        public async Task<IActionResult> NotificationTrigger([FromBody]NotificationTrigger notification)
        {
            if (notification.IsOrderV2)
            {
                await storageService.SendMessageAsync("process-order-notification", notification);
            } 
            else if(notification.IsFeedback)
            {
                await storageService.SendMessageAsync("process-delivered-notification", notification);
            }
            return Ok();
        }
    }
}
