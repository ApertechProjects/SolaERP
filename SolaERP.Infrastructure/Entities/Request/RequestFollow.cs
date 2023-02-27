using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestFollow : BaseEntity
    {
        public int RequestFollowId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        [DbColumn("UserName")]
        public string Email { get; set; }
        public int RequestMainId { get; set; }
        public string RequestNo { get; set; }
    }
}
