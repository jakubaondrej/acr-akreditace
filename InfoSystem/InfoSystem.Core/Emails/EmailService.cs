using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.Emails
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, IHostingEnvironment hostingEnvironment
            , ILogger<EmailService> logger)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task SendEmail(string email, string message, string subject, bool isHtmlBody = false)
        {
            _logger.LogInformation("Email sending...");
            _logger.LogInformation($"Email client {_emailSettings.MailServer}:{_emailSettings.MailPort}");
            SmtpClient client = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password)
            };
            
            _logger.LogInformation($"Email from {_emailSettings.SenderEmail} - {_emailSettings.SenderName}");
            MailMessage mailMessage = new MailMessage(_emailSettings.SenderEmail, email)
            {
                Body = message,
                Subject = subject,
                IsBodyHtml = isHtmlBody
            };
            try
            {
                await client.SendMailAsync(mailMessage); 
                _logger.LogInformation("Email sent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email sending failed");
            }
            client.Dispose();
        }

        public async Task SendUserCreatedEmail(string username, string password, string email)
        {
            HtmlDocument doc = new HtmlDocument();
            string file = "";
            try
            {
                file = Path.Combine(_hostingEnvironment.WebRootPath,
                           "EmailTemplates", "UserCreated.html");
                doc.Load(file);
            }
            catch
            {
                throw new Exception($"Email template could not be loaded. File path: {file}");
            }
            var htmlText = doc.Text;
            _logger.LogInformation("Replace username");
            htmlText = htmlText.Replace("{username}", username);
            _logger.LogInformation("Replace password");
            htmlText = htmlText.Replace("{password}", password);

            await SendEmail(email, htmlText, "Přístupové údaje", true);
        }
    }
}
