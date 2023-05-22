using ASM.Api.Helpers;
using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;
        private readonly IUnitOfWork uow;
        private readonly UserManager<Seller> userManager;

        public SettingsController(IMeliService meliService,
              ISellerService sellerService,
              UserManager<Seller> userManager,
              IUnitOfWork uow)
        {
            this.meliService = meliService;
            this.sellerService = sellerService;
            this.userManager = userManager;
            this.uow = uow;
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            Seller? user = uow.SellerRepository.Get(changePassword.SellerId);
            if (user == null)
            {
                return BadRequest(RequestResponse.GetError("User not found"));
            }
            var changePasswordResult = await userManager.ChangePasswordAsync(user!, changePassword.Password, changePassword.NewPassword);
            if(!changePasswordResult.Succeeded)
            {
                string errors = changePasswordResult.Errors.GetErrors();
                return BadRequest(RequestResponse.GetError(errors));
            }

            return Ok(RequestResponse.GetSuccess());
        }

        [HttpGet("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromQuery] DeleteAccount deleteAccount)
        {
            try
            {
                Seller? user = await sellerService.GetSellerInfo(deleteAccount.SellerId);
                if (user == null)
                {
                    return BadRequest(RequestResponse.GetError("User not found"));
                }

                Dictionary<bool, (MeliAccount, string)> status = await meliService.RevokeMeliAccounts(user.MeliAccounts);
                await sellerService.DeleteAccount(user);

                return Ok(RequestResponse.GetSuccess());
            }
            catch (System.Exception ex)
            {
                return BadRequest(RequestResponse.GetError("Houve um erro ao tentar deletar sua conta."));
            }
            
        }
    }
}
