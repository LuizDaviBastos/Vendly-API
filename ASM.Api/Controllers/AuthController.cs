using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Mvc;
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

        public AuthController(IMeliService meliService, AsmConfiguration asmConfiguration, ISellerService sellerService)
        {
            this.meliService = meliService;
            this.asmConfiguration = asmConfiguration;
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
        public async Task<IActionResult> SaveAccount([FromBody] SaveAccount account)
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

                if (saveResult.Item2)
                {
                    return Ok(new { success = true, data = entity });
                }

                return BadRequest(saveResult.Item1);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("SyncMeliAccount")]
        public async Task<IActionResult> SyncMeliAccount([FromBody] SynMeliAccount syncAccount)
        {
            try
            {
                var accessToken = await meliService.GetAccessTokenAsync(syncAccount.Code);
                if (accessToken.Success ?? false)
                {
                    var addResult = await sellerService.AddMeliAccount(syncAccount.SellerId, accessToken);
                    if (addResult.Item2)
                    {
                        return Ok(new { success = true });
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var login = await sellerService.Login(email, password);
                if (login.Success)
                {
                    return Ok(login);
                }

                return BadRequest(login.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
