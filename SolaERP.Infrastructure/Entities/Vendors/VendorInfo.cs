namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorInfo : BaseEntity
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
    }
}
