using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Entities.Groups
{
    public class GroupUserDto
    {
        public int GroupUserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
