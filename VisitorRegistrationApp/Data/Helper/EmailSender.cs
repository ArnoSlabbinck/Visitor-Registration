using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Helper
{
    public class EmailSender : IEmailSender
    {
        private readonly string _apiKey;
        private readonly string _name;
        private readonly string _email;

        public EmailSender(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _name = configuration["SendGrid:FromName"];
            _email = configuration["SendGrid:FromEmail"];
        }

        public async Task SendEmailAsync(string email, string subject, string Message)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_email, _name),
                Subject = subject,
                PlainTextContent = Message,
                HtmlContent = Message
            };
            msg.AddTo(new EmailAddress(_email));

            msg.SetClickTracking(false, false);
            await client.SendEmailAsync(msg);
        }
    }
}
