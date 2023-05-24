using ASM.Services.Models;
using HtmlAgilityPack;
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

            message = ReplaceWordIgnoreCaseRegex(message, "comprador", buyerName);
            message = ReplaceWordIgnoreCaseRegex(message, "produto", productTitle);
            message = ReplaceWordIgnoreCaseRegex(message, "rastreio", notification.trackingNumber);

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
            string code = "";
            var random = new Random();
            for(int i  = 0; i < 6; i++)
            {
                code += random.Next(0, 9);
            }
            return long.Parse(code);
        }
    }
}

