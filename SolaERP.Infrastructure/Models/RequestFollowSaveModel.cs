using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Models
{
    public class RequestFollowSaveModel
    {
        public int RequestMainId { get; set; }
        public int RequestFollowId { get; set; }
        public int UserId { get; set; }
        public CrudType Type { get; set; }
    }
}
