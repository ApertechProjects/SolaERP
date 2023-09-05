namespace SolaERP.Application.Entities.User
{
    public class UserList : BaseEntity
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string TemplateKey { get; set; }
        public string Language { get; set; }
        public string RequestNo { get; set; }
    }
}
