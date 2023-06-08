using ASM.Api.Models;
using ASM.Services;
using ASM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{

    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;
        private readonly IMepaService mepaService;
        private readonly ISettingsService settingsService;
        private readonly PaymentService paymentService;

        public PaymentController(IMeliService meliService, ISellerService sellerService, IMepaService mepaService, ISettingsService settingsService, PaymentService paymentService)
        {
            this.meliService = meliService;
            this.sellerService = sellerService;
            this.mepaService = mepaService;
            this.paymentService = paymentService;
            this.settingsService = settingsService;
        }

        [HttpGet("SubscriptionList")]
        public async Task<IActionResult> GetSubscriptionList() 
        {
            var subscriptions = await paymentService.GetSubscriptionPlanAsync();
            return Ok(RequestResponse.GetSuccess(subscriptions));
        }

        [HttpGet("PaymentHistory")]
        public async Task<IActionResult> GetPaymentHistory(Guid sellerId)
        {
            List<PaymentHistoryResult> history = new();
            var result = await paymentService.GetPaymentHistory(sellerId);
            if(result.Any())
            {
                history = result.Select(x => new PaymentHistoryResult(x)).ToList();
            }
            return Ok(RequestResponse.GetSuccess(history));
        }
    }
}
