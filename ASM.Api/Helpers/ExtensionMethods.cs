using ASM.Services.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace ASM.Api.Helpers
{
    public static class ExtensionMethods
    {
        public static void Set(this IConfiguration config, string key, string value)
        {
            config[$"{key}"] = value;
            //Array.ForEach(smtpConfig.GetType().GetProperties(), x => { config[$"{x.Name}"] = x.GetValue(smtpConfig).ToString(); });
        }
    }
}
