namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class VendorBuCategory : BaseEntity
    {
        public int VendorBusinessCategoryId { get; set; }
        public int VendorId { get; set; }
        public int BusinessCategoryId { get; set; }
    }
}
