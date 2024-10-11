using AngleSharp.Io;
using FluentEmail.Core;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RazorLight;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Enums;
using SolaERP.Application.Extensions;
using SolaERP.Application.Models;
using SolaERP.Application.ViewModels;
using SolaERP.Infrastructure.ViewModels;
using SolaERP.Persistence.Services;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Web;

namespace SolaERP.Infrastructure.Services
{

	public class MailService : IMailService
	{
		private readonly IConfiguration _configuration;
		private readonly IEmailNotificationService _emailNotificationService;
		private readonly IUserRepository _userRepository;
		private readonly IApproveStageService _approveStageService;
		private readonly RazorLightEngine _razorEngine;

		public MailService(IConfiguration configuration,
						   IEmailNotificationService emailNotificationService,
						   IUserRepository userRepository,
						   IApproveStageService approveStageService)
		{
			_configuration = configuration;
			_emailNotificationService = emailNotificationService;
			_userRepository = userRepository;
			_approveStageService = approveStageService;
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
    
    <p>We have received a request to set a new password for your Sola-Soft account. To ensure the security of your account, please follow the steps below to create a new password. Your security code is : <span style='color: green;'><b>@SecurityCode</b></span></p>
    
    
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
		public async Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
		{
			if (tos.Length != 0)
			{
				using (SmtpClient smtpClient = new SmtpClient())
				{
					var basicCredential = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);

					smtpClient.Host = _configuration["Mail:Host"];
					smtpClient.Port = Convert.ToInt32(_configuration["Mail:Port"]);
					smtpClient.EnableSsl = Convert.ToBoolean(_configuration["Mail:EnableSSL"]);
					smtpClient.UseDefaultCredentials = false;
					smtpClient.Credentials = basicCredential;

					using (MailMessage message = new MailMessage())
					{
						foreach (string item in tos)
						{
							message.From = new MailAddress(_configuration["Mail:UserName"], _configuration["Mail:UserAlias"]);
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
							throw;
						}
					}
				}
			}
		}

		public async Task SendManualMailsAsync(string to)
		{
			if (!string.IsNullOrEmpty(to))
			{
				using (SmtpClient smtpClient = new SmtpClient())
				{
					var basicCredential = new NetworkCredential("erpsyteline@stp.az", "Re@cti10&*(");

					smtpClient.Host = "mail.stp.az";
					smtpClient.Port = 5225;
					smtpClient.EnableSsl = true;
					smtpClient.UseDefaultCredentials = false;
					smtpClient.Credentials = basicCredential;

					using (MailMessage message = new MailMessage())
					{

						message.From = new MailAddress("erpsyteline@stp.az", _configuration["Mail:Alias"]);
						message.Subject = "Subject";
						message.IsBodyHtml = true;
						message.Body = "Test Mail";

						message.To.Add(to);

						try
						{
							await smtpClient.SendMailAsync(message);
						}
						catch (Exception ex)
						{
							throw;
						}
					}
				}
			}
		}


		public async Task<bool> SendUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName, List<string> tos)
		{
			if (tos.Count == 0)
				return false;

			var fileRootPath = Path.GetFullPath(@"wwwroot/sources/templates");
			var imageRootPath = Path.GetFullPath(@"wwwroot/sources/images");

			var engine = new RazorLightEngineBuilder()
				.UseFileSystemProject(fileRootPath)
				.EnableEncoding()
				.UseMemoryCachingProvider()
				.Build();

			string renderedHtml = await engine.CompileRenderAsync(templateName, viewModel);
			var processedBody = PreMailer.Net.PreMailer.MoveCssInline(renderedHtml, true).Html;

			Attachment? imageAttachment = null;
			if (!string.IsNullOrEmpty(imageName))
			{
				imageAttachment = new Attachment(Path.Combine(imageRootPath, imageName));
				// Attach the image file
				imageAttachment.ContentId = "image1";

				// Include the image reference in the HTML body
				processedBody = processedBody.Replace("cid:image1", $"cid:{imageAttachment.ContentId}");
			}
			using (SmtpClient smtpClient = new SmtpClient())
			{
				var basicCredential = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);

				smtpClient.Host = _configuration["Mail:Host"];
				smtpClient.Port = Convert.ToInt32(_configuration["Mail:Port"]);
				smtpClient.EnableSsl = Convert.ToBoolean(_configuration["Mail:EnableSSL"]);
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = basicCredential;

				using (MailMessage message = new MailMessage())
				{
					foreach (string item in tos)
					{
						if (IsValidEmail(item))
						{
							message.From = new MailAddress(_configuration["Mail:UserName"], _configuration["Mail:UserAlias"]);
							message.Subject = subject;
							message.IsBodyHtml = true;

							// Add the image attachment to the message
							if (message.Attachments.Count == 0 && !string.IsNullOrEmpty(imageName))
								message.Attachments.Add(imageAttachment);

							// Set the processed HTML body as the email body
							message.Body = processedBody;

							// Add recipients
							message.To.Add(item);
						}
					}

					try
					{
						await smtpClient.SendMailAsync(message);
					}
					catch (SmtpFailedRecipientException ex)
					{
						// Handle other exceptions or logging if necessary
						// ...
					}
					catch (SmtpException ex)
					{
						// Handle other exceptions or logging if necessary
						// ...
					}
				}
			}

			return true;

		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var mailAddress = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public Task<bool> SendEmailMessage<T>(string template, T viewModel, string to, string subject)
		{
			throw new NotImplementedException();
		}

		public async Task SendRequestToMailService(MailModel mailModel)
		{
			using (var client = new HttpClient())
			{
				var json = JsonSerializer.Serialize(mailModel);
				var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

				var request = await client.PostAsync(_configuration["Mail:RabbitMQUrl"] + "api/Mail", content);

			}
		}

		public async Task SendMailForRequest(HttpResponse response, List<EmailTemplateData> templates, List<UserList> users, EmailTemplateKey key, int? sequence, string businessUnitName, string rejectReason = "")
		{
			for (int i = 0; i < users.Count; i++)
			{
				var temp = templates.First(x => x.Language == users[i].Language.ToString());
				string userName = users[i].FullName;
				VM_RequestPending requestPending = new VM_RequestPending
				{
					Body = new HtmlString(temp.Body),
					Sequence = sequence,
					FullName = userName,
					Header = temp.Header,
					Subject = string.Format(temp.Subject, users[i].RequestNo, sequence),
					RequestNo = users[i].RequestNo,
					Language = users[i].Language.GetLanguageEnumValue(),
					CompanyName = businessUnitName,
					TemplateKey = key,
					ReasonDescription = rejectReason
				};
				string to = users[i].Email;
				response.OnCompleted(async () =>
				{
					await SendUsingTemplate(requestPending.Subject, requestPending, requestPending.TemplateName(), null, new List<string> { to });
				});
			};

		}

		public async Task SendRequestMailsForChangeStatus(HttpResponse response, List<UserList> users, int? sequence, string businessUnitName, string rejectReason)
		{
			var userREQP = users.Where(x => x.TemplateKey == EmailTemplateKey.REQP.ToString()).ToList();
			if (userREQP.Count > 0)
			{
				var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQP);
				await SendMailForRequest(response, templates, userREQP, EmailTemplateKey.REQP, sequence, businessUnitName);
			}
			var userREQA = users.Where(x => x.TemplateKey == EmailTemplateKey.REQA.ToString()).ToList();
			if (userREQA.Count > 0)
			{
				var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQA);
				await SendMailForRequest(response, templates, userREQA, EmailTemplateKey.REQA, sequence, businessUnitName);

			}
			var userREQR = users.Where(x => x.TemplateKey == EmailTemplateKey.REQR.ToString()).ToList();
			if (userREQR.Count > 0)
			{
				var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQR);
				await SendMailForRequest(response, templates, userREQR, EmailTemplateKey.REQR, sequence, businessUnitName, rejectReason);
			}
			var userREQH = users.Where(x => x.TemplateKey == EmailTemplateKey.REQH.ToString()).ToList();
			if (userREQH.Count > 0)
			{
				var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQH);
				await SendMailForRequest(response, templates, userREQH, EmailTemplateKey.REQH, sequence, businessUnitName);
			}
		}

		public async Task SendRegistrationPendingMail(int userId)
		{
			User user = await _userRepository.GetByIdAsync(userId);
			var companyName = await _emailNotificationService.GetCompanyName(user.Email);

			var templateDataForRegistrationPending =
				await _emailNotificationService.GetEmailTemplateData(user.Language, EmailTemplateKey.RGA);

			VM_RegistrationPending registrationPending = new VM_RegistrationPending()
			{
				FullName = user.FullName,
				UserName = user.UserName,
				Header = templateDataForRegistrationPending.Header,
				Body = new HtmlString(string.Format(templateDataForRegistrationPending.Body, user.FullName)),
				Language = user.Language,
				CompanyName = companyName,
			};

			await SendUsingTemplate(templateDataForRegistrationPending.Subject,
				   registrationPending,
				   registrationPending.TemplateName(),
				   registrationPending.ImageName(),
				   new List<string> { user.Email });

		}

		public async Task SendMailToAdminstrationForApproveRegistration(int userId, List<string> details = null)
		{
			User user = await _userRepository.GetByIdAsync(userId);
			var companyName = await _emailNotificationService.GetCompanyName(user.Email);
			List<Task> emails = new List<Task>();
			var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.RP);
			foreach (var lang in Enum.GetValues<Language>())
			{
				var sendUserMails = await _userRepository.GetAdminUserMailsAsync(1, lang);
				if (sendUserMails.Count > 0)
				{
					var templateData = templates.First(x => x.Language == lang.ToString());
					VM_RegistrationIsPendingAdminApprove adminApprove = new(details)
					{
						Body = new HtmlString(templateData.Body),
						CompanyName = companyName,
						Header = templateData.Header,
						UserName = user.UserName,
						CompanyOrVendorName = companyName,
						Language = templateData.Language.GetLanguageEnumValue(),
					};

					Task RegEmail = SendUsingTemplate(templateData.Subject, adminApprove,
						adminApprove.TemplateName, adminApprove.ImageName, sendUserMails);
					emails.Add(RegEmail);
				}
			}

		}

		public async Task CheckLastApproveStageAndSendMailToVendor(int vendorId, int sequence, int approveStatus, HttpResponse response)
		{
			var stageCount = await _approveStageService.GetStageCountAsync(Procedures.Vendors);
			if (stageCount == sequence && approveStatus == 1)
			{
				var vendorUser = await _userRepository.GetUserByVendor(vendorId);
				var companyName = await _emailNotificationService.GetCompanyName(vendorUser.Email);

				VM_VendorApprove vendorApprove = new VM_VendorApprove(vendorUser.Language.ToString())
				{
					CompanyName = companyName,
					Language = (Language)Enum.Parse(typeof(Language), vendorUser.Language.ToString())
				};


				response.OnCompleted(async () =>
				{
					await SendUsingTemplate(vendorApprove.Subject, vendorApprove,
					vendorApprove.TemplateName(), null,
					new List<string> { vendorUser.Email });
				});

			}
		}

		public async Task SendEmailVerification(HttpResponse response, int userId)
		{
			User user = await _userRepository.GetByIdAsync(userId);
			var templateDataForVerification =
				  _emailNotificationService.GetEmailTemplateData(user.Language, EmailTemplateKey.VER).Result;
			var companyName = _emailNotificationService.GetCompanyName(user.Email).Result;

			VM_EmailVerification emailVerification = new VM_EmailVerification
			{
				Username = user.UserName,
				Body = new HtmlString(string.Format(templateDataForVerification.Body, user.FullName)),
				CompanyName = companyName,
				Header = templateDataForVerification.Header,
				Language = user.Language,
				Subject = templateDataForVerification.Subject,
				Token = HttpUtility.HtmlDecode(user.VerifyToken),
			};

			response.OnCompleted(async () =>
			{
				await SendUsingTemplate(templateDataForVerification.Subject, emailVerification,
					emailVerification.TemplateName(), emailVerification.ImageName(),
					new List<string> { user.Email });
			});
		}

		public async Task SendMailToAdminstrationAboutRegistration(int userId)
		{
			User user = await _userRepository.GetByIdAsync(userId);
			var companyName = await _emailNotificationService.GetCompanyName(user.Email);
			List<Task> emails = new List<Task>();
			var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.AAI);
			foreach (var lang in Enum.GetValues<Language>())
			{
				var sendUserMails = await _userRepository.GetAdminUserMailsAsync(1, lang);
				if (sendUserMails.Count > 0)
				{
					var templateData = templates.First(x => x.Language == lang.ToString());
					VM_InformationAboutRegistrationForAdmin adminApprove = new()
					{
						Body = new HtmlString(string.Format(templateData.Body, user.FullName, user.Email)),
						CompanyName = companyName,
						Header = templateData.Header,
						UserName = user.UserName,
						CompanyOrVendorName = companyName,
						Language = templateData.Language.GetLanguageEnumValue(),

					};

					Task RegEmail = SendUsingTemplate(templateData.Subject, adminApprove,
						adminApprove.TemplateName, adminApprove.ImageName, sendUserMails);
					emails.Add(RegEmail);
				}
			}
		}
	}


}

