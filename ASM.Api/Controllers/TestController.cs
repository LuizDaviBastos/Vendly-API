using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly FcmService fcmService;

        public TestController(IConfiguration configuration, FcmService fcmService)
        {
            this.configuration = configuration;
            this.fcmService = fcmService;
        }

        [HttpGet(nameof(GetLocalConfiguration))]
        public IActionResult GetLocalConfiguration()
        {
            return Ok();
        }

        [HttpGet(nameof(TestNotification))]
        public async Task<IActionResult> TestNotification(string fcmToken)
        {
            await fcmService.SendNotificationAsync(fcmToken);
            return Ok();
        }
    }
}
