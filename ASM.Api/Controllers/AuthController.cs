using ASM.Api.Helpers;
using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWork uow;
        private readonly AsmConfiguration asmConfiguration;
        private readonly UserManager<Seller> userManager;

        public AuthController(IMeliService meliService,
            AsmConfiguration asmConfiguration,
            ISellerService sellerService,
            UserManager<Seller> userManager,
            IUnitOfWork uow)
        {
            this.meliService = meliService;
            this.asmConfiguration = asmConfiguration;
            this.sellerService = sellerService;
            this.userManager = userManager;
            this.uow = uow;
        }

        [HttpGet("GetAuthUrl")]
        public async Task<IActionResult> GetAuthUrl(string countryId)
        {
            if (string.IsNullOrEmpty(countryId) || !asmConfiguration.Countries.ContainsKey(countryId.ToUpper())) return BadRequest("invalid country");

            string authUrl = await meliService.GetAuthUrl(countryId, null);
            return Ok(RequestResponse.GetSuccess(authUrl, string.Empty));
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
                    UserName = account.Email,
                    Country = account.Country,
                    BillingInformation = new PaymentInformation
                    {
                        ExpireIn = DateTime.UtcNow.AddDays(15)
                    }
                };

                var createResult = await userManager.CreateAsync(entity, account.Password);
                if (createResult.Succeeded)
                {
                    var loginResponse = await GetLoginResponseAsync(new() { Email = account.Email, Password = account.Password });
                    return Ok(RequestResponse.GetSuccess(loginResponse));
                }

                string errors = createResult.Errors.GetErrors();
                return BadRequest(RequestResponse.GetError(errors));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("SendEmailConfirmation")]
        public async Task<IActionResult> SendEmailConfirmation(Guid sellerId)
        {
            try
            {
                (string message, bool status) = await sellerService.SendEmailConfirmationCode(sellerId);
                if(status)
                {
                    return Ok(RequestResponse.GetSuccess());
                }

                return Ok(RequestResponse.GetError(message));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(Guid sellerId, string confirmationCode)
        {
            try
            {
                (string message, bool status) = await sellerService.ConfirmEmailAsync(sellerId, confirmationCode);
                if (status)
                {
                    return Ok(RequestResponse.GetSuccess());
                }

                return Ok(RequestResponse.GetError(message));
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
        public async Task<IActionResult> IsAuthenticated(string token, Guid sellerId)
        {
            try
            {
                bool tokenIsValid = Uteis.IsAuthenticated(token, asmConfiguration);
                return Ok(tokenIsValid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //call to sync meli account
        [HttpGet("SyncMeli")]
        public async Task<IActionResult> SyncMeli(string countryId, string token, bool signup)
        {
            try
            {
                if (Uteis.TryGetUserId(token, out Guid sellerId))
                {
                    var url = await meliService.GetAuthUrl(countryId, new() { SellerId = sellerId, Signup = signup });
                    return Redirect(url);
                }

                return BadRequest(RequestResponse.GetError("")); //TODO redirect to Error Razor Page
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //dont call
        [HttpGet("SyncMeliAccount")]
        public async Task<IActionResult> SyncMeliAccount(string code, string? state)
        {
            try
            {
                if (StateUrl.TryGetState(state, out StateUrl stateOut) && !string.IsNullOrEmpty(code))
                {
                    var accessToken = await meliService.GetAccessTokenAsync(code);
                    if (accessToken.Success ?? false)
                    {
                        var addResult = await sellerService.AddMeliAccount(stateOut.SellerId, accessToken);
                        if (addResult.Item2)
                        {
                            if(stateOut.Signup)
                            {
                                return Redirect($"asm.app://auth/signup?step=5&sync=true");
                            }
                            else
                            {
                                return Redirect($"asm.app://message");
                            }
                            
                        }
                        string path = $"auth/signup?step=5&sync=false&error=true&message={System.Net.WebUtility.UrlEncode(addResult.Item1)}";
                        return Redirect($"asm.app://{path}");
                        //return BadRequest(RequestResponse.GetError(addResult.Item1)); //TODO redirect to Error Razor Page
                    }

                    return BadRequest(RequestResponse.GetError(accessToken.Message));
                }

                return BadRequest(RequestResponse.GetError("User id not found"));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPassword(string email)
        {
            try
            {
                var result = await sellerService.SendEmailRecoveryPassword(email);
                if(result.Item2)
                {
                    return Ok(RequestResponse.GetSuccess());
                }

                return BadRequest(RequestResponse.GetError(result.Item1));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("RedirectConfirmRecoveryPassword")]
        public async Task<IActionResult> RedirectConfirmRecoveryPassword(Guid sellerId, string code)
        {
            try
            {
                return Redirect($"asm.app://auth/confirm-recovery-password?sellerId={sellerId}&code={code}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ConfirmRecoveryPassword")]
        public async Task<IActionResult> ConfirmRecoveryPassword([FromBody] RecoveryPassword recoveryPassword)
        {
            try
            {
                var result = await sellerService.RecoveryPassword(recoveryPassword.SellerId, recoveryPassword.Code, recoveryPassword.NewPassword);
                if (result.Item2)
                {
                    return Ok(RequestResponse.GetSuccess());
                }

                return BadRequest(RequestResponse.GetError(result.Item1));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AuthSignup")]
        public IActionResult AuthSignup()
        {
            return Redirect($"asm.app://auth/signup?step=5");
        }

        private async Task<LoginResponse> GetLoginResponseAsync(LoginRequest login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, login.Password))
            {
                user.MeliAccounts = uow.MeliAccountRepository.GetQueryable().Include(x => x.Messages).Where(x => x.SellerId == user.Id).ToList();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("UserId", user.Id.ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(asmConfiguration.JwtKey)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                LoginResponse loginResponse = new()
                {
                    Data = user,
                    Token = tokenHandler.WriteToken(token),
                    HasMeliAccount = await sellerService.HasMeliAccount(user.Id),
                    Success = true,
                };

                if (!user.EmailConfirmed)
                {
                    loginResponse.EmailNotConfirmed = true;
                    loginResponse.Message = "Email não verificado.";
                }

                return loginResponse;
            }

            return new() { Success = false, Message = "Usuario ou senha inválido" };
        }
    }
}
