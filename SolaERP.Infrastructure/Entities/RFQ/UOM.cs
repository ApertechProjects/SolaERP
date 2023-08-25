using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.RFQ
{
    public class UOM : BaseEntity
    {
        [DbColumn("UOM")]
        public string UnitOfMeasure { get; set; }
        public decimal Conv_Factor { get; set; }
    }
}
