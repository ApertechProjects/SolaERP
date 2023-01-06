namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorRepresentedCompany : BaseEntity
    {
        public int VendorRepresentedCompanyId { get; set; }
        public int VendorId { get; set; }
        public string RepresentedCompanyName { get; set; }
    }
}
