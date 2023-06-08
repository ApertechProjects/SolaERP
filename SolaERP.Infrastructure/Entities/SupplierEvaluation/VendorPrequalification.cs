namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class VendorPrequalification : BaseEntity
    {
        public int VendorPrequalificationCategoryId { get; set; }
        public int VendorId { get; set; }
        public int PrequalificationCategoryId { get; set; }
    }
}
