using ASM.Services.Interfaces;
using FCM.Net;

namespace ASM.Services
{
    public class FcmService
    {
        private ISellerService sellerService;
        private ISettingsService settingsService;
        private string key = "AAAA-uIs9KA:APA91bHYvwU7PLfmU6ADoMgYqgnrwDAzUlC5EFgI8tKBAgHaDhHQY-QvrK8VYYlEsAHoAeugyxdPxv1j6s_oRgRcakCsrtxRCDEG0vLlTgZYJAxaWKwc8mKvM79BxJVUvh5jyp-f-PTS";
        public FcmService(ISellerService sellerService, ISettingsService settingsService)
        {
            this.sellerService = sellerService;
            this.settingsService = settingsService;
        }

        public async Task SendNotificationForAllAsync(string title, string body)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            var tokens = await sellerService.GetAllFcmTokensAsync();
            using (var sender = new Sender(settings.FcmServerKey ?? key))
            {
                var message = new Message
                {
                    RegistrationIds = new List<string>(tokens),
                    Notification = new Notification
                    {
                        Title = title ?? "Test from FCM.Net",
                        Body = body ??$"Hello World!{DateTime.Now.ToString()}"
                    }
                };
                var result = await sender.SendAsync(message);
            }
        }


        public async Task SendNotificationAsync(string fcmToken, bool showForUsers = true)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            using (var sender = new Sender(settings.FcmServerKey ?? key))
            {
                var message = new Message
                {
                    RegistrationIds = new List<string> { fcmToken },
                    Notification = new Notification
                    {
                        Tag = (showForUsers ? "show" : string.Empty),
                        Title = "Test from FCM.Net",
                        Body = $"Hello World@!{DateTime.Now.ToString()}"
                    }
                };
                var result = await sender.SendAsync(message);
            }
        }

        public async Task SendNotificationAsync(Guid sellerId, string title, string body, bool showForUsers = true, Priority priority = Priority.Normal)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            var tokens = await sellerService.GetFcmTokensAsync(sellerId);
            using (var sender = new Sender(settings.FcmServerKey ?? key))
            {
                var message = new Message
                {
                    RegistrationIds = tokens,
                    Priority = priority,
                    Notification = new Notification
                    {
                        Tag = (showForUsers ? "show" : string.Empty),
                        Title = title,
                        Body = body
                    },
                };

                
                var result = await sender.SendAsync(message);
            }
        }
    }
}
