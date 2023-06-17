using ASM.Api.Models;
using ASM.Services;
using ASM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : AsmBaseController
    {
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;
        private readonly IMepaService mepaService;
        private readonly ISettingsService settingsService;
        private readonly PaymentService paymentService;

        public AccountController(IMeliService meliService, ISellerService sellerService, IMepaService mepaService, ISettingsService settingsService, PaymentService paymentService)
        {
            this.meliService = meliService;
            this.sellerService = sellerService;
            this.mepaService = mepaService;
            this.settingsService = settingsService;
            this.paymentService = paymentService;
        }

        [HttpGet("GetSellerInfo")]
        public async Task<IActionResult> GetSellerInfo(Guid sellerId)
        {
            try
            {
                var sellerInfo = await sellerService.GetSellerAndMeliAccounts(sellerId);

                if (sellerInfo != null) return Ok(RequestResponse.GetSuccess(sellerInfo));
                return BadRequest(RequestResponse.GetError("Usuario não encontrado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMeliSellerInfo")]
        public async Task<IActionResult> GetMeliSellerInfo(long? meliSellerId)
        {
            try
            {
                if(TryGetSellerId(out Guid sellerId))
                {
                    if (!meliSellerId.HasValue || meliSellerId == 0)
                    {
                        meliSellerId = await sellerService.GetFirstMeliAccountId(sellerId);
                    }
                }
                
                var sellerInfo = await meliService.GetMeliSellerInfo(meliSellerId.Value);
                sellerInfo.Messages = await sellerService.GetMessagesByMeliSellerId(meliSellerId.Value);
                if (sellerInfo.Success ?? false) return Ok(RequestResponse.GetSuccess(sellerInfo));
                return BadRequest(RequestResponse.GetError(sellerInfo.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("HasMeliAccount")]
        public async Task<IActionResult> HasMeliAccount(Guid sellerId)
        {
            try
            {
                bool hasMeliAccount = await sellerService.HasMeliAccount(sellerId);
                return Ok(hasMeliAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

        [HttpGet("ExpiredStatus")]
        public async Task<IActionResult> ExpiredStatus(Guid sellerId)
        {
            try
            {
                var status = await paymentService.ExpirateDateValid(sellerId);
                return Ok(RequestResponse.GetSuccess(status));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("fcmToken")]
        public async Task<IActionResult> FcmToken([FromBody] RegisterFcmToken fcmToken)
        {
            try
            {
                await sellerService.RegisterFcmToken(fcmToken.SellerId, fcmToken.FcmToken);
                return Ok(RequestResponse.GetSuccess());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPaymentInformations")]
        public async Task<IActionResult> GetPaymentInformations(Guid sellerId)
        {
            try
            {
                var pInfo = await paymentService.GetPaymentInformation(sellerId);
                if (pInfo == null || !pInfo.ExpireIn.HasValue) return Ok(RequestResponse.GetError("Informações da assinatura não encontradas."));

                PaymentInformationResult response = new PaymentInformationResult(pInfo, sellerId);

                //var settings = await settingsService.GetAppSettingsAsync();
                //response.Price = settings?.VendlyItem?.Price;

                return Ok(RequestResponse.GetSuccess(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
