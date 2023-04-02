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
    public class AccountController : Controller
    {
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;

        public AccountController(IMeliService meliService, ISellerService sellerService)
        {
            this.meliService = meliService;
            this.sellerService = sellerService;
        }

        [HttpGet("GetSellerInfo")]
        public async Task<IActionResult> GetSellerInfo(Guid sellerId)
        {
            try
            {
                var sellerInfo = await sellerService.GetSellerInfo(sellerId);

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
    }
}
