namespace SolaERP.Infrastructure.Entities.Email
{
    public class EmailNotification : BaseEntity
    {
        public bool Check { get; set; }
        public int EmailNotificationId { get; set; }
        public string Notification { get; set; }
        public string Description { get; set; }
    }
}
