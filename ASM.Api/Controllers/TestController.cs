using ASM.Services;
using ASM.Services.Interfaces;
using ASM.Services.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
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
        private readonly IEmailService emailService;


        public TestController(IConfiguration configuration, FcmService fcmService, IMepaService mepaService, ISellerService sellerService, IEmailService emailService)
        {
            this.configuration = configuration;
            this.fcmService = fcmService;
            this.mepaService = mepaService;
            this.sellerService = sellerService;
            this.emailService = emailService;
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

        [HttpGet(nameof(SendEmail))]
        public async Task<IActionResult> SendEmail(string to, string? url)
        {
            await emailService.SendEmail(to, "Vendly - Test", HtmlTemplates.EmailRecoveryPassword, new Dictionary<string, string> { { "url", url ?? "https://facebook.com" } });
            return Ok();
        }
    }
}
