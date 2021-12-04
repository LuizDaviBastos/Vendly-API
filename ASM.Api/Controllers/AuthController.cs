using ASM.Services.Helpers;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMeliService meliService;
        private readonly AsmConfiguration asmConfiguration;

        public AuthController(IMeliService meliService, AsmConfiguration asmConfiguration)
        {
            this.meliService = meliService;
            this.asmConfiguration = asmConfiguration;
        }

        [HttpGet("GetAuthUrl")]
        public IActionResult GetAuthUrl(string countryId)
        {
            if (string.IsNullOrEmpty(countryId) || !asmConfiguration.Countries.ContainsKey(countryId.ToUpper())) return BadRequest("invalid country");

            return Ok(meliService.GetAuthUrl(countryId));
        }

        [HttpGet("GetAccessToken")]
        public async Task<IActionResult> GetAccessToken(string code)
        {
            return Ok(await meliService.GetAccessTokenAsync(code));
        }
    }
}
