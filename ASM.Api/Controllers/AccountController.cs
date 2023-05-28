using ASM.Api.Models;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public AccountController(IMeliService meliService, ISellerService sellerService, IMepaService mepaService)
        {
            this.meliService = meliService;
            this.sellerService = sellerService;
            this.mepaService = mepaService;
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
        public async Task<IActionResult> GetMeliSellerInfo(long meliSellerId)
        {
            try
            {
                var sellerInfo = await meliService.GetMeliSellerInfo(meliSellerId);

                if(sellerInfo.Success ?? false) return Ok(RequestResponse.GetSuccess(sellerInfo));
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

        [HttpGet("GetPaymentLink")]
        public async Task<IActionResult> GetPaymentLink(Guid sellerId)
        {
            try
            {
                var createResponse = await mepaService.CreatePayment(sellerId);
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

        [HttpGet("ExpiredStatus")]
        public async Task<IActionResult> ExpiredStatus(Guid sellerId)
        {
            try
            {
                var status = await sellerService.ExpirateDateValid(sellerId);
                return Ok(RequestResponse.GetSuccess(status));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
