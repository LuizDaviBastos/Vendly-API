using ASM.Services.Models;
using HtmlAgilityPack;

namespace ASM.Services.Helpers
{
    public class Utils
    {
        public static string PrepareSellerMessage(string message, Order order)
        {
            string productTitle = order.order_items?.FirstOrDefault()?.item?.title ?? string.Empty;
            string? buyerName = order?.buyer?.first_name;

            message = SetMentionValue(message, "mention-COMPRADOR", buyerName);
            message = SetMentionValue(message, "mention-PRODUTO", productTitle);
            message = SetMentionValue(message, "mention-RASTREIO", productTitle);
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
    }
}

