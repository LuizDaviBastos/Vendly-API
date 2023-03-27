using ASM.Api.Helpers;
using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;
        private readonly AsmConfiguration asmConfiguration;
        private readonly IConfiguration configuration;

        public AuthController(IMeliService meliService, AsmConfiguration asmConfiguration, IConfiguration configuration, ISellerService sellerService)
        {
            this.meliService = meliService;
            this.asmConfiguration = asmConfiguration;
            this.configuration = configuration;
            this.sellerService = sellerService;
        }

        [HttpGet("GetAuthUrl")]
        public IActionResult GetAuthUrl(string countryId)
        {
            if (string.IsNullOrEmpty(countryId) || !asmConfiguration.Countries.ContainsKey(countryId.ToUpper())) return BadRequest("invalid country");

            return Ok(new
            {
                url = meliService.GetAuthUrl(countryId)
            });
        }

        [HttpPost("SaveAccount")]
        public async Task<IActionResult> SaveAccount(SaveAccount account)
        {
            try
            {
                var entity = new Seller
                {
                    Email = account.Email,
                    Password = account.Password,
                    FirstName = account.FirstName,
                    LastName = account.LastName
                };
                var saveResult = await sellerService.SaveAccount(entity);

                if(saveResult.Item2)
                {
                    return Ok(entity);
                }

                return BadRequest(saveResult.Item2);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("SyncMeliAccount")]
        public async Task<IActionResult> SyncMeliAccount(string code, Guid sellerId)
        {
            try
            {
                var accessToken = await meliService.GetAccessTokenAsync(code);
                if (accessToken.Success ?? false)
                {
                    var addResult = await sellerService.AddMeliAccount(sellerId, accessToken);
                    if (addResult.Item2)
                    {
                        return Ok(addResult.Item1);
                    }

                    return BadRequest(addResult.Item1);
                }

                return BadRequest(accessToken.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetSellerInfoBySellerId")]
        public async Task<IActionResult> GetSellerInfo(long meliSellerId)
        {
            try
            {
                var sellerInfo = await meliService.GetSellerInfoByMeliSellerId(meliSellerId);

                if (sellerInfo.Success ?? false) return Ok(sellerInfo);

                return BadRequest(sellerInfo);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet("SetSettings")]
        public IActionResult SetSettings(string key, string value)
        {
            configuration.Set(key, value);
            return Ok();
        }

        [HttpGet("GetSettings")]
        public IActionResult GetSettings(string key)
        {
            return Ok(configuration[key]);
        }

        [HttpGet("LoginSuccess")]
        public IActionResult LoginSuccess(string code)
        {
            return View();
            /*
            var html = System.IO.File.ReadAllText("./Views/Auth/LoginSuccess.html");
            return Content(html, "text/html");*/
        }
    }
}
