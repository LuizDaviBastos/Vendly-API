using ASM.Core.Services;
using ASM.Imp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMeliService meliService;
        public AuthController(IMeliService meliService)
        {
            this.meliService = meliService;
        }

        [HttpGet("GetAuthUrl")]
        public IActionResult GetAuthUrl(string countryId)
        {
            if (string.IsNullOrEmpty(countryId) || !MLConstants.Countries.ContainsKey(countryId)) return BadRequest("invalid country");

            return Ok(meliService.GetAuthUrl(countryId));
        }

        [HttpGet("GetAccessToken")]
        public async Task<IActionResult> GetAccessToken(string code)
        {
            return Ok(await meliService.GetAccessTokenAsync(code));
        }
    }
}
