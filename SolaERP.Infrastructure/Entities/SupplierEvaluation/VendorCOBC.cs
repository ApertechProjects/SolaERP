namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class VendorCOBC : BaseEntity
    {
        public int VendorCOBCId { get; set; }
        public int VendorId { get; set; }
        public int BusinessUnitId { get; set; }
    }
}
