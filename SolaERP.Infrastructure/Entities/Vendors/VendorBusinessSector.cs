namespace SolaERP.Application.Entities.Vendors
{
    public class VendorBusinessSector : BaseEntity
    {
        public int VendorBusinessSectorId { get; set; }
        public int BusinessSectorId { get; set; }
        public string BusinessSectorName { get; set; }
    }
}
