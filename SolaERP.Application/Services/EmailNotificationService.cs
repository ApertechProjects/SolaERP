using AutoMapper;
using Microsoft.AspNetCore.Html;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Email;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Infrastructure.ViewModels;
using System.Web;

namespace SolaERP.Persistence.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        IEmailNotificationRepository _emailNotificationRepository;
        private IMapper _mapper;
        public EmailNotificationService(IEmailNotificationRepository emailNotificationRepository, IMapper mapper)
        {
            _emailNotificationRepository = emailNotificationRepository;
            _mapper = mapper;
        }
        public async Task<string> GetCompanyName(string email)
        {
            var res = await _emailNotificationRepository.GetCompanyName(email);
            return res;
        }

        public async Task<EmailTemplateDataDto> GetEmailTemplateData(Language language, EmailTemplateKey templateKey)
        {
            var data = await _emailNotificationRepository.GetEmailTemplateData(language, templateKey);
            var result = _mapper.Map<EmailTemplateDataDto>(data);
            return result;
        }

        public async Task<List<EmailTemplateData>> GetEmailTemplateData(EmailTemplateKey templateKey)
        {
            var templates = await _emailNotificationRepository.GetEmailTemplateData(templateKey);
            return templates;
        }

        //public async Task<bool> SendVerificationEmail(UserRegisterModel dto)
        //{
        //    var templateDataForVerification = await GetEmailTemplateData(dto.Language, EmailTemplateKey.VER);
        //    var companyName = await GetCompanyName(dto.Email);

        //    VM_EmailVerification emailVerification = new VM_EmailVerification()
        //    {
        //        Username = dto.UserName,
        //        Body = new HtmlString(string.Format(templateDataForVerification.Body, dto.FullName)),
        //        CompanyName = companyName,
        //        Header = templateDataForVerification.Header,
        //        Language = dto.Language,
        //        Subject = templateDataForVerification.Subject,
        //        Token = HttpUtility.HtmlDecode(dto.VerifyToken),
        //    };

        //    Response.OnCompleted(async () =>
        //    {
        //        await _mailService.SendUsingTemplate(templateDataForVerification.Subject, emailVerification, emailVerification.TemplateName(), emailVerification.ImageName(), new List<string> { dto.Email });
        //    });
        //}
    }
}
