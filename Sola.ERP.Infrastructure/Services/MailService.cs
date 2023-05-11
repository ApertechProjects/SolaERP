using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RazorLight;
using SolaERP.Application.Contracts.Services;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace SolaERP.Infrastructure.Services
{

    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private const string TemplatePath = @"SolaERP.API/wwwroot/sources/templates/RegistrationPending.cshtml";
        private IFluentEmail _email;
        private readonly ILogger<MailService> _logger;
        private readonly RazorLightEngine _razorEngine;

        public MailService(IConfiguration configuration, IFluentEmail email, ILogger<MailService> logger)
        {
            //var rootPath = Path.GetFullPath(@"wwwroot/sources/templates");
            _configuration = configuration;
            _email = email;
            _logger = logger;
            //_razorEngine = new RazorLightEngineBuilder()
            //.UseFileSystemProject(rootPath)
            //.UseMemoryCachingProvider()
            //.Build();
        }

        public Task<bool> SendEmailMessage<T>(string template, T viewModel, string to, string subject)
        {
            throw new NotImplementedException();
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
        public async Task SendPasswordResetMailAsync(string to, string code)
        {

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

                    #region Template Starts 
                    string Template = @"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<style>
  body {
    font-family: Arial, sans-serif;
    font-size: 16px;
    line-height: 1.5;
    color: #333;
    background-color: #f5f5f5;
  }
  
  .email-container {
    max-width: 600px;
    margin: 30px auto;
    padding: 20px;
    background-color: #fff;
    border-radius: 5px;
  }
  
  h1 {
    font-size: 24px;
    font-weight: bold;
    color: #444;
    margin-bottom: 20px;
  }
  
  p {
    margin-bottom: 15px;
  }
  
  .cta-button {
    display: inline-block;
    font-weight: bold;
    text-decoration: none;
    padding: 12px 25px;
    background-color: #0077cc;
    color: #fff;
    border-radius: 5px;
    margin-top: 20px;
  }
  
  .cta-button:hover {
    background-color: #005fa3;
  }
  
  .footer {
    font-size: 14px;
    color: #999;
    margin-top: 30px;
  }
</style>
</head>
<body>
  <div class=""email-container"">
    <h1>Set Your New Password for Sola-Soft Account</h1>
    
    <p>Dear Customer,</p>
    
    <p>We have received a request to set a new password for your Sola-Soft account. To ensure the security of your account, please follow the steps below to create a new password. Your security code is : @SecurityCode</p>
    
    
    <p>If you did not request a new password, please contact our support team immediately at support@apertech.net to secure your account.</p>
    
    <p>Thank you for choosing Sola-Soft!</p>
    
    <p>Best regards,</p>
    
    <p>Apertech Support Team</p>
    
    <p class=""footer"">This email was sent to you because a password reset request was made for your Sola-Soft account. If you did not make this request, please contact our support team to secure your account.</p>
  </div>
</body>
</html>
";
                    #endregion 

                    message.Body = Template.Replace("@SecurityCode", code);

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
                            message.From = new MailAddress(_configuration["Mail:UserName"], "Apertech");
                            message.Subject = subject;
                            message.IsBodyHtml = isBodyHtml;
                            message.Body = body;

                            message.To.Add(item);
                        }

                        try
                        {
                            await smtpClient.SendMailAsync(message);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }


        public async Task<bool> SendUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName, List<string> tos)
        {
            #region
            //var rootPath = Path.GetFullPath(@"wwwroot/sources/templates");

            //var engine = new RazorLightEngineBuilder()
            //.UseFileSystemProject(rootPath)
            //.EnableEncoding()
            //.UseMemoryCachingProvider()
            //.Build();


            //string renderedHtml = await engine.CompileRenderAsync(templateName, viewModel);

            //var processedBody = PreMailer.Net.PreMailer.MoveCssInline(renderedHtml, true).Html;
            //Email.DefaultSender = _email.Sender;
            //_email = Email
            //.From("hulya.garibli@apertech.net")
            //.Subject(subject)
            //.UsingTemplate(processedBody, viewModel);


            //foreach (var item in tos)
            //{
            //    try
            //    {
            //        _email.To(item);
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //var response = await _email.SendAsync();
            //return response.Successful;
            #endregion
            var fileRootPath = Path.GetFullPath(@"wwwroot/sources/templates");
            var imageRootPath = Path.GetFullPath(@"wwwroot/Content/image");

            LinkedResource imageResource = new LinkedResource(imageRootPath + "\\" + imageName);
            imageResource.ContentId = "image1";

            var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(fileRootPath)
            .EnableEncoding()
            .UseMemoryCachingProvider()
            .Build();

            string renderedHtml = await engine.CompileRenderAsync(templateName, viewModel);
            var processedBody = PreMailer.Net.PreMailer.MoveCssInline(renderedHtml, true).Html;
            processedBody = processedBody.Replace("<br>", "\n");
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(processedBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(imageResource);



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
                        message.From = new MailAddress(_configuration["Mail:UserName"], "Apertech");
                        message.Subject = subject;
                        message.IsBodyHtml = true;
                        message.Body = processedBody;

                        message.AlternateViews.Add(alternateView);
                        message.To.Add(item);
                    }

                    try
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return true;


        }

    }
}
