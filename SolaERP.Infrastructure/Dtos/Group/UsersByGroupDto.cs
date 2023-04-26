namespace SolaERP.Infrastructure.Dtos.Group
{
    public class UsersByGroupDto
    {
        public int GroupUserId { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool IsInGroup { get; set; }
    }
}
