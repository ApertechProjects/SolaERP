namespace SolaERP.Application.Entities.Vendors
{
    public class VendorPrequalificationCategory : BaseEntity
    {
        public int VendorPrequalificationCategoryId { get; set; }
        public int VendorId { get; set; }
        public int PrequalificationCategoryId { get; set; }

    }
}
