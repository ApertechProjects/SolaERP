using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Controllers;
using SolaERP.Infrastructure.ViewModels;
using System.Web;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EmailController : CustomBaseController
    {
        private readonly IMailService _mailService;
        private readonly IEmailNotificationService _emailNotificationService;
        public EmailController(IMailService mailService,
                               IEmailNotificationService emailNotificationService)
        {
            _mailService = mailService;
            _emailNotificationService = emailNotificationService;
        }


        [HttpPost]
        public async Task<IActionResult> SendVerificationEmail(UserRegisterModel dto)
        {
            var templateDataForVerification = await _emailNotificationService.GetEmailTemplateData(dto.Language, EmailTemplateKey.VER);
            var companyName = await _emailNotificationService.GetCompanyName(dto.Email);

            VM_EmailVerification emailVerification = new VM_EmailVerification()
            {
                Username = dto.UserName,
                Body = new HtmlString(string.Format(templateDataForVerification.Body, dto.FullName)),
                CompanyName = companyName,
                Header = templateDataForVerification.Header,
                Language = dto.Language,
                Subject = templateDataForVerification.Subject,
                Token = HttpUtility.HtmlDecode(dto.VerifyToken),
            };

            Response.OnCompleted(async () =>
            {
                await _mailService.SendUsingTemplate(templateDataForVerification.Subject, emailVerification, emailVerification.TemplateName(), emailVerification.ImageName(), new List<string> { dto.Email });
            });
            return CreateActionResult(ApiResponse<bool>.Success(200));
        }
    }
}
