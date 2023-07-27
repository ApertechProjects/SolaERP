namespace SolaERP.Application.Entities.RFQ
{
    public class RfqVendor : BaseEntity
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Email { get; set; }
    }
}
