using ASM.Services;
using ASM.Services.Interfaces;
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
        private readonly IMepaService mepaService;
        private readonly ISellerService sellerService;

        public TestController(IConfiguration configuration, FcmService fcmService, IMepaService mepaService, ISellerService sellerService)
        {
            this.configuration = configuration;
            this.fcmService = fcmService;
            this.mepaService = mepaService;
            this.sellerService = sellerService;
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

        [HttpGet(nameof(TestNotificationForAll))]
        public async Task<IActionResult> TestNotificationForAll(string title, string body)
        {
            await fcmService.SendNotificationForAllAsync(title, body);
            return Ok();
        }

        [HttpGet(nameof(CreatePreference))]
        public async Task<IActionResult> CreatePreference(Guid sellerId)
        {
            var response = await mepaService.CreatePreference(sellerId);
            return Ok(response);
        }

        [HttpGet(nameof(GetPayment))]
        public async Task<IActionResult> GetPayment(Guid sellerId)
        {
           //var result = mepaService.GetPreference(sellerId);
            return Ok();
        }
    }
}
