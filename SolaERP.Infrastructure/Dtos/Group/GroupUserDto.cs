using SolaERP.Application.Entities.Auth;

namespace SolaERP.Application.Entities.Groups
{
    public class GroupUserDto
    {
        public int GroupUserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool UserInGroup { get; set; }
    }
}
