using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities.Layout
{
    public class Layout : BaseEntity
    {
        [DbIgnore]
        public int UserLayoutId { get; set; }

        [DbIgnore]
        public int UserId { get; set; }

        [DbIgnore]
        public string Key { get; set; }

        [DbColumn("Layout")]
        public byte[] UserLayout { get; set; }
        public int TabIndex { get; set; }
    }
}
