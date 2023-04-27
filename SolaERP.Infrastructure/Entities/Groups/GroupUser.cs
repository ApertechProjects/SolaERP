namespace SolaERP.Application.Entities.Groups
{
    public class GroupUser : BaseEntity
    {
        public int GroupUserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool UserInGroup { get; set; }
    }
}
