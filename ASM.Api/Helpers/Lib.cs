using ASM.Services.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace ASM.Api.Helpers
{
    public class Uteis
    {
        public static bool IsAuthenticated(string token, AsmConfiguration asmConfiguration)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(asmConfiguration.JwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                handler.ValidateToken(token, validationParameters, out var securityToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool TryGetUserId(string jwtToken, out Guid UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);

            string? userId = token.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            return Guid.TryParse(userId, out UserId);
        }
    }
}
