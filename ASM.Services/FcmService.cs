using ASM.Services.Interfaces;
using FCM.Net;

namespace ASM.Services
{
    public class FcmService
    {
        private ISettingsService settingsService;
        private string key = "AAAA-uIs9KA:APA91bHYvwU7PLfmU6ADoMgYqgnrwDAzUlC5EFgI8tKBAgHaDhHQY-QvrK8VYYlEsAHoAeugyxdPxv1j6s_oRgRcakCsrtxRCDEG0vLlTgZYJAxaWKwc8mKvM79BxJVUvh5jyp-f-PTS";
        public FcmService(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public async Task SendPushNotificationAsync(List<string> tokens, string title, string body, Priority priority = Priority.Normal)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            using (var sender = new Sender(settings.FcmServerKey ?? key))
            {
                var message = new Message
                {
                    RegistrationIds = tokens,
                    Priority = priority,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                };

                var result = await sender.SendAsync(message);
            }
        }
    }
}
