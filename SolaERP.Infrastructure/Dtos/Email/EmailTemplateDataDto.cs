namespace SolaERP.Application.Dtos.Email
{
    public class EmailTemplateDataDto
    {
        public int EmailTemplateId { get; set; }
        public string Subject { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }
}
