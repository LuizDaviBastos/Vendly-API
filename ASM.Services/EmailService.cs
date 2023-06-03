using ASM.Services.Interfaces;
using ASM.Services.Models.Settings;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

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

            await send(clientId, clientSecret, tenantId, userName, to, subject, body);

            //return;

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




        public async Task send(string clientId, string clientSecret, string tenantId, string from, string to, string subject, string body)
        {
            string accessToken = await GetAccessToken(clientId, clientSecret, tenantId);

            // Configuração da mensagem

            // Criação do objeto de e-mail
            var message = new Root
            {
                message = new Message2()
                {
                    subject = subject,
                    body = new Body
                    {
                        content = body,
                        contentType = "HTML"
                    },
                    toRecipients = new List<ToRecipient>
                    {
                        new ToRecipient
                        {
                            EmailAddress = new EmailAddress2 { Address = to }
                        }
                    }
                }
            };

            // Serializa o objeto de e-mail em JSON
            string json = JsonConvert.SerializeObject(message);

            // URL para enviar o e-mail usando a API do Microsoft Graph
            string sendEmailUrl = $"https://graph.microsoft.com/v1.0/users/91e73383-f3f7-4349-8475-fb3ce569705c/sendMail";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Configuração do cabeçalho de autenticação
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Configuração do conteúdo da requisição
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Envio do e-mail
                    HttpResponseMessage response = await client.PostAsync(sendEmailUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("E-mail enviado com sucesso!");
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Erro ao enviar o e-mail: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar o e-mail: " + ex.Message);
            }
        }


        static async Task<string> GetAccessToken(string clientId, string clientSecret, string tenantId)
        {
            // URL para obter o token de acesso
            string tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Configuração do conteúdo da requisição
                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default")
                });

                    // Requisição para obter o token de acesso
                    HttpResponseMessage response = await client.PostAsync(tokenUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(jsonResponse);
                        string accessToken = data.access_token;
                        return accessToken;
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Erro ao obter o token de acesso: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter o token de acesso: " + ex.Message);
            }

            return null;
        }
    }
}
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Body
{
    public string contentType { get; set; }
    public string content { get; set; }
}

public class EmailAddress2
{
    public string Address { get; set; }
}

public class InternetMessageHeader
{
    public string name { get; set; }
    public string value { get; set; }
}

public class Message2
{
    public string subject { get; set; }
    public Body body { get; set; }
    public List<ToRecipient> toRecipients { get; set; }
    //public List<InternetMessageHeader> internetMessageHeaders { get; set; }
}

public class Root
{
    public Message2 message { get; set; }
}

public class ToRecipient
{
    public EmailAddress2 EmailAddress { get; set; }
}

