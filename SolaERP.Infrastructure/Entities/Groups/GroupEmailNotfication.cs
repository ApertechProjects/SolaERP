namespace SolaERP.Application.Entities.Groups
{
    public class GroupEmailNotification : BaseEntity
    {
        public int GroupEmailNotificationId { get; set; }
        public int GroupId { get; set; }
        public int EmailNotificationId { get; set; }
        public string Notification { get; set; }
        public string Description { get; set; }
        public bool IsInGroup { get; set; }
    }
}
