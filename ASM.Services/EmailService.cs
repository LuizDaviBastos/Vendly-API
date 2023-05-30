using ASM.Services.Interfaces;
using ASM.Services.Models.Settings;
using System.Net;
using System.Net.Mail;

namespace ASM.Services
{
    public class EmailService : IEmailService
    {
        private SmtpSettings smtpSettings;
        private readonly ISettingsService settingsService;

        public EmailService(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public async Task SendEmail(string to, string body, string subject)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            smtpSettings = settings.SmtpSettings;
            var basicCredential = new NetworkCredential(smtpSettings.Email, smtpSettings.Password);
            MailAddress fromAddress = new MailAddress(smtpSettings.Email, smtpSettings.UserName);

            var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port.Value);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;

            var mailMessage = new MailMessage(fromAddress, new(to))
            {
                Subject = subject,
                Priority = MailPriority.Normal,
                Body = body,
                IsBodyHtml = true,
                DeliveryNotificationOptions = DeliveryNotificationOptions.None,
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
