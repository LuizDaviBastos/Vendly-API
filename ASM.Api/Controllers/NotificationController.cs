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
        private readonly IMeliService meliService;

        public NotificationController(IStorageService storageService, IMeliService meliService)
        {
            this.storageService = storageService;
            this.meliService = meliService;
        }

        [HttpPost("NotificationTrigger")]
        public async Task<IActionResult> NotificationTrigger([FromBody]NotificationTrigger notification)
        {
            await storageService.SendMessageAsync("pre-process-notifications", notification);
            return Ok();
        }
    }
}
