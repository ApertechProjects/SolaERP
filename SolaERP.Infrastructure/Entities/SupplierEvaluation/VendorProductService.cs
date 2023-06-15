using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class VendorProductService : BaseEntity
    {
        [DbColumn("VendorProductService")]
        public int VendorProductServiceId { get; set; }
        public int VendorId { get; set; }
        public int ProductServiceId { get; set; }
    }
}
