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

    }
}
