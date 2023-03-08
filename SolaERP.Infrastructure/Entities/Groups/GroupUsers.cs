using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Entities.Groups
{
    public class GroupUsers
    {
        public int GroupUserId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public SolaERP.Infrastructure.Entities.Auth.User User { get; set; }
    }
}
