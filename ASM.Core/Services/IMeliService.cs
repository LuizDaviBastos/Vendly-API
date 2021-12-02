using ASM.Core.Models;

namespace ASM.Core.Services
{
    public interface IMeliService
    {
        public string GetAuthUrl(string countryId);
        public Task<AccessToken> GetAccessTokenAsync(string code);
        public Task<AccessToken> RefreshAccessTokenAsync(string refreshToken, long sellerId);
        public Task<Order> GetOrderDetailsAsync(NotificationTrigger notification, bool tryAgayn = true);
        public Task SendMessageToBuyerAsync(SendMessage sendMessage, bool tryAgayn = true);
        public Task<TResult> RefreshTokenAndTryAgain<TResult>(string refreshToken, long sellerId, Func<Task<TResult>> func);
    }
}
