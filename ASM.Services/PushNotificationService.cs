using ASM.Data.Interfaces;
using FCM.Net;
using Microsoft.EntityFrameworkCore;

namespace ASM.Services
{
    public class PushNotificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FcmService fcmService;

        public PushNotificationService(IUnitOfWork unitOfWork, FcmService fcmService)
        {
            this.unitOfWork = unitOfWork;
            this.fcmService = fcmService;
        }

        public async Task SendPushNotificationAsync(Guid sellerId, string title, string body, long? id = null, Priority priority = Priority.Normal)
        {
            var tokens = await unitOfWork.SellerFcmTokenRepository.GetTokens(sellerId);
            await fcmService.SendPushNotificationAsync(tokens, title, body, id, priority);
        }

        public async Task SendPushNotificationAsync(long meliSellerId, string title, string body, long? id = null, Priority priority = Priority.Normal)
        {
            var sellerId = await unitOfWork.MeliAccountRepository.GetQueryable().Where(x => x.MeliSellerId == meliSellerId).Select(x => x.SellerId).FirstOrDefaultAsync();
            if(sellerId.HasValue)
            {
                await SendPushNotificationAsync(sellerId.Value, title, body, id, priority); 
            }
        }
    }

}
