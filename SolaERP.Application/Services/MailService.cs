using Microsoft.Extensions.Configuration;
using SolaERP.Infrastructure.Contracts.Services;
using System.Net;
using System.Net.Mail;

namespace SolaERP.Application.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private string ResetVrificationCode { get; set; }

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Username"], "Apertech", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_configuration["Mail:Username"]);

                    smtpClient.Host = _configuration["Mail:Host"];
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;

                    message.From = fromAddress;
                    message.Subject = "Reset Password";
                    message.IsBodyHtml = true;
                    Random rand = new Random();
                    ResetVrificationCode = rand.Next(100000, 999999).ToString();
                    message.Body = ResetVrificationCode;
                    message.To.Add(to);
                    await smtpClient.SendMailAsync(message);
                }
            }
        }
    }
}
