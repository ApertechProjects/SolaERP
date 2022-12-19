using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Entities.Chat
{
    public class GroupChat : BaseEntity
    {
        public int Id { get; set; }
        public string Groupname { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
