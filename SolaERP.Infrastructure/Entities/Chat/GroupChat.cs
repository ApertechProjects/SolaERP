namespace SolaERP.Application.Entities.Chat
{
    public class GroupChat : BaseEntity
    {
        public int Id { get; set; }
        public string Groupname { get; set; }
        public int UserId { get; set; }
        public SolaERP.Application.Entities.Auth.User User { get; set; }
    }
}
