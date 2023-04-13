namespace SolaERP.Infrastructure.Entities.Groups
{
    public class GroupEmailNotfication : BaseEntity
    {
        public int GroupEmailNotificationId { get; set; }
        public int GroupId { get; set; }
        public int EmailNotificationId { get; set; }
    }
}
