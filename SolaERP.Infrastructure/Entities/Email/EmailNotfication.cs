namespace SolaERP.Infrastructure.Entities.Email
{
    public class EmailNotfication : BaseEntity
    {
        public bool Check { get; set; }
        public int EmailNotificationId { get; set; }
        public string Notification { get; set; }
        public string Description { get; set; }
    }
}
