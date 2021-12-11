using ASM.Api.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMeliService meliService;
        private readonly AsmConfiguration asmConfiguration;
        private readonly IConfiguration configuration;

        public AuthController(IMeliService meliService, AsmConfiguration asmConfiguration, IConfiguration configuration)
        {
            this.meliService = meliService;
            this.asmConfiguration = asmConfiguration;
            this.configuration = configuration;
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

        [HttpGet("GetAccessToken")]
        public async Task<IActionResult> GetAccessToken(string code)
        {
            try
            {
                var accessToken = await meliService.GetAccessTokenAsync(code);

                if (accessToken.Success ?? false) return Ok(accessToken);

                return BadRequest(accessToken);
            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("GetSellerInfo")]
        public async Task<IActionResult> GetSellerInfo(string accessToken)
        {
            try
            {
                var sellerInfo = await meliService.GetSellerInfo(accessToken);

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
            var html = System.IO.File.ReadAllText("./Views/Auth/LoginSuccess.html");
            return Content(html, "text/html");
        }
    }
}
