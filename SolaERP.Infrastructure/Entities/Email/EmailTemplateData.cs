namespace SolaERP.Application.Entities.Email
{
    public class EmailTemplateData : BaseEntity
    {
        public int EmailTemplateId { get; set; }
        public string Subject { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }
}
