using ASM.Services.Models;
using Azure.Storage.Blobs.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ASM.Services.Helpers
{
    public class Utils
    {
        public static string PrepareSellerMessage(string message, Order order, NotificationTrigger notification)
        {
            string productTitle = order.order_items?.FirstOrDefault()?.item?.title ?? string.Empty;
            string? buyerName = order?.buyer?.first_name;

            message = SetMentionValue(message, "mention-COMPRADOR", buyerName);
            message = SetMentionValue(message, "mention-PRODUTO", productTitle);
            message = SetMentionValue(message, "mention-RASTREIO", notification.trackingNumber);
            message = SetMentionValue(message, "mention-RASTREIOURL", notification.trackingUrl);

            message = ReplaceWordIgnoreCaseRegex(message, "comprador", buyerName);
            message = ReplaceWordIgnoreCaseRegex(message, "produto", productTitle);
            message = ReplaceWordIgnoreCaseRegex(message, "rastreio", notification.trackingNumber);
            message = ReplaceWordIgnoreCaseRegex(message, "rastreiourl", notification.trackingUrl);

            return message;
        }

        private static string SetMentionValue(string html, string flag, string? value)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var divElement = doc.DocumentNode.SelectSingleNode($"//label[@name='{flag}']");
            if (divElement != null)
            {
                divElement.Name = "i";
                divElement.InnerHtml = value ?? string.Empty;
                divElement.Attributes.RemoveAll();

                return doc.DocumentNode.OuterHtml;
            }

            return html;
        }

        private static string ReplaceWordIgnoreCaseRegex(string input, string flag, string? value)
        {
            string pattern = $@"@((?i){flag})\b";
            string replacedText = Regex.Replace(input, pattern, value ?? "");
            return replacedText;
        }

        public static long GetRandomCode()
        {
            var random = new Random();
            return random.Next(100000, 999999);
        }

        public static string GetBase64String<T>(T value)
        {
            var json = JsonConvert.SerializeObject(value);
            return GetBase64String(json);
        }

        public static string GetBase64String(string value) 
        {
           return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        public static T? GetFromBase64String<T>(string baseString)
        {
            string value = GetFromBase64String(baseString);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string GetFromBase64String(string baseString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(baseString));
        }
    }
}

