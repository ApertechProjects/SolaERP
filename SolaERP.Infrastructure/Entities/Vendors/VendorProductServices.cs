namespace SolaERP.Application.Entities.Vendors
{
    public class VendorProductServices : BaseEntity
    {
        public int VendorProductService { get; set; }
        public int VendorId { get; set; }
        public int ProductServiceId { get; set; }
    }
}
