using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.UOM
{
    public class UOM : BaseEntity
    {
        [DbColumn("UOM")]
        public string UOM_Name { get; set; }
        public string Description { get; set; }
    }
}
