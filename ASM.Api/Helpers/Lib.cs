using ASM.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ASM.Api.Helpers
{
    public static class Uteis
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

        public static string NormalizeFileName(string fileName, string extension)
        {
            if (string.IsNullOrEmpty(fileName)) return GetSystemFileName(extension);

            string invalidChars = new string(Path.GetInvalidFileNameChars());
            fileName = string.Concat(fileName.Split(invalidChars.ToCharArray()));
            fileName = fileName.Replace(" ", "-");
            fileName = fileName.ToLowerInvariant();
            return fileName;
        }

        private static string GetSystemFileName(string? extension = null)
        {
            var date = DateTime.UtcNow;
            return $"file-{date.Year}{date.Month}{date.Day}{date.Hour}{date.Minute}{date.Second}{date.Millisecond}{(extension?.HasValue() ?? false ? ($".{extension}") : string.Empty )}";
        }

        public static string FormatFileSize(long fileSizeInBytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int index = 0;
            double fileSize = fileSizeInBytes;
            while (fileSize >= 1024 && index < units.Length - 1)
            {
                fileSize /= 1024;
                index++;
            }
            return string.Format("{0:0.##} {1}", fileSize, units[index]);
        }

        public static string GetFirstName(string fullName)
        {
            return fullName?.Split(" ")?[0] ?? "";
        }

        public static string GetErrors(this IEnumerable<IdentityError> erros)
        {
            string output = string.Empty;
            foreach (var error in erros)
            {
                output += $"{error.Description} \n";
            }
            return output;
        }
    }
}
