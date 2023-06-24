using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Services;
using ASM.Services.Interfaces;
using ASM.Services.Models;
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
        public async Task<IActionResult> GetSubscriptionList(bool? isFree) 
        {
            var subscriptions = await paymentService.GetSubscriptionPlanAsync(isFree);
            return Ok(RequestResponse.GetSuccess(subscriptions));
        }

        [HttpGet("CreatePaymentLink")]
        public async Task<IActionResult> CreatePaymentLink(Guid sellerId, Guid subscriptionPlanId, bool isBinary)
        {
            try
            {
                var createResponse = await paymentService.GetNewPaymentLink(sellerId, subscriptionPlanId, isBinary);
                if (!createResponse.Success ?? true)
                {
                    return Ok(RequestResponse.GetError(createResponse.Message));
                }

                return Ok(RequestResponse.GetSuccess(createResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPaymentLink")]
        public async Task<IActionResult> GetPaymentLink(Guid historyId)
        {
            try
            {
                var paymentLink = await paymentService.GetPaymentLink(historyId);
                if (!paymentLink.Success ?? true)
                {
                    return Ok(RequestResponse.GetError(paymentLink.Message));
                }

                return Ok(RequestResponse.GetSuccess(paymentLink));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PaymentHistory")]
        public async Task<IActionResult> GetPaymentHistory([FromQuery] Guid sellerId, int skip, int take)
        {
            List<PaymentHistoryResult> history = new();
            (List<PaymentHistory> result, long total) = await paymentService.GetPaymentHistory(sellerId, skip, take);
            if(result.Any())
            {
                
                history = result.Select(x => new PaymentHistoryResult(x)).ToList();
            }
            return Ok(RequestResponse.GetSuccess(history, total: total));
        }
    }
}
