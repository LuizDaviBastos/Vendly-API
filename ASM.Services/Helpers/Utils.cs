using ASM.Services.Models;

namespace ASM.Services.Helpers
{
    public class Utils
    {
        public static string PrepareSellerMessage(string message, Order order)
        {
            return message.Replace("@COMPRADOR", order?.buyer?.first_name);
        }
    }
}
