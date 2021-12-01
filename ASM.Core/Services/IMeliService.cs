using ASM.Core.Models;

namespace ASM.Core.Services
{
    public interface IMeliService
    {
        public string GetAuthUrl(string countryId);
        public Task<AccessToken> GetAccessTokenAsync(string code);
        public Task<AccessToken> RefreshAccessTokenAsync(string refreshToken);
        public Task<Order> GetOrderDetailsAsync(NotificationTrigger notification);
        public Task SendMessageToBuyerAsync(SendMessage sendMessage);
    }
}
