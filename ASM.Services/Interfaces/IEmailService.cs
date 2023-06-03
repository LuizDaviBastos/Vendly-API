﻿namespace ASM.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(string to, string body, string subject);
        public Task SendEmailMsGraph(string to, string body, string subject);
    }
}
