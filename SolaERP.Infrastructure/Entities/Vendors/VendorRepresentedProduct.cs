namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorRepresentedProduct : BaseEntity
    {
        public int VendorRepresentedProductId { get; set; }
        public int VendorId { get; set; }
        public string RepresentedProductName { get; set; }
    }
}
