using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities.Layout
{
    public class Layout : BaseEntity
    {
        [DbIgnore]
        public int UserId { get; set; }
        public string Filebase64 { get; set; }
        [DbIgnore]
        public string Key { get; set; }

        [DbColumn("Layout")]
        public byte[] UserLayout { get; set; }
        public int TabIndex { get; set; }
    }
}
