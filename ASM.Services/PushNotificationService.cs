using ASM.Data.Interfaces;
using FCM.Net;

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

        public async Task SendPushNotificationAsync(Guid sellerId, string title, string body, Priority priority = Priority.Normal)
        {
            var tokens = await unitOfWork.SellerFcmTokenRepository.GetTokens(sellerId);
            await fcmService.SendPushNotificationAsync(tokens, title, body, priority);
        }
    }

}
