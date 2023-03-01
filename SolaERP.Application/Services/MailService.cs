using Microsoft.Extensions.Configuration;
using SolaERP.Infrastructure.Contracts.Services;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;

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
            if (tos.Length != 0)
            {
                MailMessage mail = new();
                mail.IsBodyHtml = isBodyHtml;

                foreach (var to in tos)
                    mail.To.Add(to);

                mail.Subject = subject;
                mail.Body = body;
                mail.From = new(_configuration["Mail:UserName"], "Apertech", System.Text.Encoding.UTF8);

                SmtpClient smtp = new();
                smtp.Credentials = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Host = _configuration["Mail:Host"];

                await smtp.SendMailAsync(mail);
            }
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
                var basicCredential = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_configuration["Mail:UserName"]);

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
            List<string> failedList = new List<string>();

            if (tos.Length == 0)
            {
                return failedList;
            }

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);

                smtpClient.Host = "mail.apertech.net";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("test@apertech.com");
                    message.Subject = subject;
                    message.IsBodyHtml = isBodyHtml;
                    message.Body = body;

                    foreach (string item in tos)
                    {
                        message.CC.Add(item);
                    }

                    try
                    {
                        await Task.Run(async () =>
                        {
                            await smtpClient.SendMailAsync(message);
                        });
                    }
                    catch (SmtpException ex)
                    {
                        failedList.Add(ex.Message);
                    }
                }
            }

            return failedList;
        }

        public async Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            if (tos.Length != 0)
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    var basicCredential = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);

                    smtpClient.Host = "mail.apertech.net";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;

                    using (MailMessage message = new MailMessage())
                    {
                        foreach (string item in tos)
                        {
                            message.From = new MailAddress("test@apertech.com","Apertech");
                            message.Subject = subject;
                            message.IsBodyHtml = isBodyHtml;
                            message.Body = body;

                            message.To.Add(item);
                        }

                        try
                        {
                            await Task.Run(async () =>
                            {
                                await smtpClient.SendMailAsync(message);
                            });
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }
    }
}
