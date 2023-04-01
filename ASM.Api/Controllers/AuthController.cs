﻿using ASM.Api.Helpers;
using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMeliService meliService;
        private readonly ISellerService sellerService;
        private readonly AsmConfiguration asmConfiguration;
        private readonly UserManager<Seller> userManager;

        public AuthController(IMeliService meliService, 
            AsmConfiguration asmConfiguration, 
            ISellerService sellerService, 
            UserManager<Seller> userManager)
        {
            this.meliService = meliService;
            this.asmConfiguration = asmConfiguration;
            this.sellerService = sellerService;
            this.userManager = userManager;
        }

        [HttpGet("GetAuthUrl")]
        public IActionResult GetAuthUrl(string countryId)
        {
            if (string.IsNullOrEmpty(countryId) || !asmConfiguration.Countries.ContainsKey(countryId.ToUpper())) return BadRequest("invalid country");

            return Ok(RequestResponse.GetSuccess(meliService.GetAuthUrl(countryId, null), "success"));
        }

        [HttpPost("SaveAccount")]
        public async Task<IActionResult> SaveAccount([FromBody] SaveAccount account)
        {
            try
            {
                var entity = new Seller
                {
                    Email = account.Email,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    UserName = account.Email
                };

                var createResult = await userManager.CreateAsync(entity, account.Password);
                if (createResult.Succeeded)
                {
                    var loginResponse = await GetLoginResponseAsync(new() { Email = account.Email, Password = account.Password });
                    return Ok(RequestResponse.GetSuccess(loginResponse));
                }

                string errors = string.Empty;
                foreach (var error in createResult.Errors)
                {
                    errors += $"{error.Description} \n";
                }

                return BadRequest(RequestResponse.GetError(errors));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                var loginResponse = await GetLoginResponseAsync(login);
                if (loginResponse.Success)
                {
                    return Ok(RequestResponse.GetSuccess(loginResponse));
                }

                return Unauthorized(RequestResponse.GetError(loginResponse.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("IsAuthenticated")]
        public IActionResult IsAuthenticated(string token)
        {
            try
            {
                var result = Uteis.IsAuthenticated(token, asmConfiguration);
                if (result) return Ok(true);

                return Unauthorized(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SyncMeliAccount")]
        public async Task<IActionResult> SyncMeliAccount(string code, string? state)
        {
            try
            {
                if(StateUrl.TryGetState(state, out StateUrl stateOut) && !string.IsNullOrEmpty(code)) 
                {
                    var accessToken = await meliService.GetAccessTokenAsync(code);
                    if (accessToken.Success ?? false)
                    {
                        var addResult = await sellerService.AddMeliAccount(stateOut.SellerId, accessToken);
                        if (addResult.Item2)
                        {
                            return Redirect("asm.app://message");
                        }

                        return BadRequest(RequestResponse.GetError(addResult.Item1)); //TODO redirect to Error Razor Page
                    }

                    return BadRequest(RequestResponse.GetError(accessToken.Message));
                }
                
                return BadRequest(RequestResponse.GetError("User id not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("SyncMeli")]
        [Authorize]
        public IActionResult SyncMeli()
        {
            try
            {
                if (Guid.TryParse(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value, out Guid sellerId))
                {
                    var url = this.meliService.GetAuthUrl("br", new() { SellerId = sellerId});
                    return Redirect(url);
                }

                return BadRequest(RequestResponse.GetError("")); //TODO redirect to Error Razor Page
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<LoginResponse> GetLoginResponseAsync(LoginRequest login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, login.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("UserId", user.Id.ToString())
                }),
                    Expires = DateTime.UtcNow.AddMilliseconds(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(asmConfiguration.JwtKey)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                LoginResponse loginResponse = new()
                {
                    Data = user,
                    Token = tokenHandler.WriteToken(token),
                    HasMeliAccount = await sellerService.HasMeliAccount(user.Id),
                    Success = true
                };

                return loginResponse;
            }

            return new() { Success = false, Message = "Ususario ou senha inválido" };
        }
    }
}
