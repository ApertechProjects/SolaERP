using SolaERP.Application.Enums;

namespace SolaERP.Application.Models
{
    public class MailModel
    {
        public string Subject { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public EmailTemplateKey EmailType { get; set; }
        public List<string> Tos { get; set; }
    }
}
