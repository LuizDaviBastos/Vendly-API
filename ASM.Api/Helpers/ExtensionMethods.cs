using ASM.Api.Models;
using ASM.Data.Entities;
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


        public static Seller ToEntity(this UpdateMessage updateMessage)
        {
            return new()
            {
                Message = updateMessage?.Message,
                AfterSellerMessageEnabled = updateMessage?.AfterSellerMessageEnabled,
                SellerId = updateMessage?.SellerId ??0
            };
        }

        public static UpdateMessage ToUpdateMessage(this Seller seller)
        {
            return new()
            {
                Message = seller?.Message ?? "",
                AfterSellerMessageEnabled = seller?.AfterSellerMessageEnabled,
                SellerId = seller?.SellerId ?? 0
            };
        }
    }
}
