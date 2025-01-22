using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Enums;
using SolaERP.Application.Helper;

namespace SolaERP.Infrastructure.ViewModels
{
    public class VM_Support : VM_EmailTemplateBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _lang;
        private readonly string _subject;
        private readonly string _body;
        private readonly string _userEmail;
        private readonly string _attachmentText;
        public VM_Support(string lang, string subject, string body, string userEmail, string attachmentText)
        {
            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
            _lang = lang;
			_subject = subject;
			_body = body;
			_userEmail = userEmail;
            _attachmentText = attachmentText;
        }

        public string? Token { get; set; }
        public string? Subject
        {
            get
            {
                return _subject;

			}
        }
        public string? Username { get; set; }


        public string TemplateName()
        {
            return @"Support.cshtml";
        }

        public string GetHeaderOfMail()
        {
            return "User email : " + _userEmail;
        }

        public HtmlString GetBodyOfMail()
        {
            return new HtmlString(_body);
        }

		public HtmlString GetAttachmentLinksOfMail()
		{
			return new HtmlString(_attachmentText);
		}


	}
}
