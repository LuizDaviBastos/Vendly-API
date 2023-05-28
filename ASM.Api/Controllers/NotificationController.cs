using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IStorageService storageService;
        private readonly IMeliService meliService;
        private readonly IMepaService mepaService;

        public NotificationController(IStorageService storageService, IMeliService meliService, IMepaService mepaService)
        {
            this.storageService = storageService;
            this.meliService = meliService;
            this.mepaService = mepaService;
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
