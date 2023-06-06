namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class VendorNDA : BaseEntity
    {
        public int VendorNDAId { get; set; }
        public int VendorId { get; set; }
        public int BusinessUnitId { get; set; }
    }
}
