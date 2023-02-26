namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestFollow : BaseEntity
    {
        public int RequestFollowId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int RequestMainId { get; set; }
    }
}
