using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Entities.Groups
{
    public class GroupUsers
    {
        public int GroupUserId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
