namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class VendorPrequalification : BaseEntity
    {
        public int VendorPrequalificationCategoryId { get; set; }
        public int VendorId { get; set; }
        public int PrequalificationCategoryId { get; set; }
    }

    public class VendorPrequalificationValues : ValueEntity
    {
        public int VendorPrequalificationId { get; set; }
        public int PrequalificationDesignId { get; set; }
        public int VendorPrequalificationCategoryId { get; set; }
    }
}
