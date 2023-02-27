using Microsoft.Extensions.Configuration;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;
using System.Net;
using System.Net.Mail;

namespace SolaERP.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
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
            mail.From = new(_configuration["Mail:Email"], "Apertech", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Email"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];

            await smtp.SendMailAsync(mail);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, Func<string[]> _onExceptionCaught, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Email"], "Apertech", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Email"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);

        }

        /// <summary>
        /// Sends Password Reset email 
        /// </summary>
        /// <param name="to">Email receiver</param>
        /// <param name="templatePath">Template path for sendin email </param>
        /// <returns></returns>
        public async Task SendPasswordResetMailAsync(string to, string templatePath)
        {
            using StreamReader reader = new StreamReader(templatePath);
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
                    message.Subject = "Email Verification for Reset Password";
                    message.IsBodyHtml = true;

                    message.Body = reader.ReadToEnd();
                    message.To.Add(to);

                    await smtpClient.SendMailAsync(message);
                }
            }
        }

        public async Task<List<string>> SendSafeMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;

            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Email"], "Apertech", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Email"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];

            List<string> unSuccesfulEmails = new();


            foreach (var to in tos)
            {
                try
                {

                    mail.To.Add(to);
                    await smtp.SendMailAsync(mail);

                }
                catch (Exception ex)
                {
                    unSuccesfulEmails.Add(to);
                }
            }

            return unSuccesfulEmails;
        }

    }
}
