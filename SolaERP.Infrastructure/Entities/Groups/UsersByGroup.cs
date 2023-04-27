namespace SolaERP.Application.Entities.Groups
{
    public class UsersByGroup : BaseEntity
    {
        public int GroupUserId { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int Gender { get; set; }
        public bool IsInGroup { get; set; }
    }
}
