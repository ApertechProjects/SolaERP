using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.User
{
    public class ActiveUser : BaseEntity
    {
        [DbColumn("Id")]
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}
