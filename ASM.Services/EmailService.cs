using ASM.Services.Interfaces;
using ASM.Services.Models.Settings;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using RestSharp;
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

        public async Task SendEmail(string to, string subject, string templateName, Dictionary<string, string> @params)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            smtpSettings = settings.SmtpSettings;

            RestClient client = new RestClient("https://api.elasticemail.com");
            RestRequest request = new("/v2/email/send", Method.POST);

            request.AddParameter("apikey", smtpSettings.ElasticEmailApiKey);
            request.AddParameter("from", smtpSettings.UserName);
            request.AddParameter("fromName", smtpSettings.Name);
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("isTransactional", true);
            request.AddParameter("template", templateName);

            foreach (var item in @params)
            {
                request.AddParameter($"merge_{item.Key}", item.Value);
            }

            var result = await client.ExecuteAsync(request);
        }

        public async Task SendEmailSMTP(string to, string body, string subject)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            smtpSettings = settings.SmtpSettings;

            var basicCredential = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password);
            MailAddress fromAddress = new MailAddress(smtpSettings.Email, smtpSettings.Name);

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
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendEmailMsGraph(string to, string body, string subject)
        {
            var settings = await settingsService.GetAppSettingsAsync();
            smtpSettings = settings.SmtpSettings;
            var graphSettings = smtpSettings.MicrosoftGraph;

            var scopes = new List<string> { "https://graph.microsoft.com/.default" };
            string tenantId = graphSettings.TenantId;
            string clientId = graphSettings.ClientId;
            string clientSecret = graphSettings.ClientSecret;
            string userName = smtpSettings.UserName;

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var message = new Message()
            {
                Subject = subject,
                Body = new ItemBody
                {
                    Content = body,
                    ContentType = BodyType.Html
                },
                ToRecipients = new List<Recipient> { new Recipient { EmailAddress = new EmailAddress { Address = to } } }
            };

            await graphClient.Users[userName].SendMail.PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
            {
                Message = message
            });
        }



    }
}
