using ASM.Services.Models;

namespace ASM.Services.Interfaces
{
    public interface IMeliService
    {
        public string GetAuthUrl(string countryId);
        public Task<AccessToken> GetAccessTokenAsync(string code);
        public Task<Order> GetOrderDetailsAsync(NotificationTrigger notification, bool tryAgain = true);
        public Task<bool> SendMessageToBuyerAsync(SendMessage sendMessage, bool tryAgain = true);
        public Task<bool> IsFirstSellerMessage(SendMessage sendMessage, bool tryAgain = true);
        public Task<SellerInfo> GetMeliSellerInfo(long sellerId, bool tryAgain = true);
    }
}
