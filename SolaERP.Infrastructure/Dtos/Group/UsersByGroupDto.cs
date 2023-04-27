namespace SolaERP.Application.Dtos.Group
{
    public class UsersByGroupDto
    {
        public int GroupUserId { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool Gender { get; set; }
        public bool IsInGroup { get; set; }
    }
}
