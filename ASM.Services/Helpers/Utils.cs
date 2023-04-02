using ASM.Services.Models;

namespace ASM.Services.Helpers
{
    public class Utils
    {
        public static string PrepareSellerMessage(string message, Order order)
        {
            string productTitle = order.order_items?.FirstOrDefault()?.item?.title ?? string.Empty;
            return message.Replace("@COMPRADOR", order?.buyer?.first_name).Replace("@PRODUTO", productTitle);
        }
    }
}
