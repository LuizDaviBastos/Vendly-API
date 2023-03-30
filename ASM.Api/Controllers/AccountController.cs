using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
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

                return Ok(sellerInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMeliSellerInfo")]
        public async Task<IActionResult> GetSellerInfo(long meliSellerId)
        {
            try
            {
                var sellerInfo = await meliService.GetSellerInfoByMeliSellerId(meliSellerId);

                if(sellerInfo.Success ?? false) return Ok(sellerInfo);
                return BadRequest(sellerInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
