using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Webhook.Controllers
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
        public async Task<IActionResult> NotificationTrigger([FromBody] NotificationTrigger notification)
        {
            await storageService.SendMessageAsync("pre-process-notifications", notification);
            return Ok();
        }

        [HttpPost("NotificationTriggerPayments")]
        public async Task<IActionResult> NotificationTriggerPayments([FromBody] NotificationTriggerPayments notification)
        {
            await storageService.SendMessageAsync("process-payments", notification);
            return Ok();
        }
    }
}
