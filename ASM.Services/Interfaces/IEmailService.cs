namespace ASM.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailSMTP(string to, string body, string subject);
        public Task SendEmail(string to, string subject, string templateName, Dictionary<string, string> @params);
    }
}
