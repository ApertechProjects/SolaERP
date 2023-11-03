namespace SolaERP.Application.Entities.Vendors
{
    public class BaseVendor : BaseEntity
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TaxId { get; set; }
        public string PrequalificationCategory { get; set; }
        public string BusinessCategory { get; set; }
        public string ProductService { get; set; }
        public string Country { get; set; }
        public string CompanyAddress { get; set; }
        public int ReviseNo { get; set; }
        public DateTime ReviseDate { get; set; }
        public string Description { get; set; }
        public string DefaultCurrency { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string CountryName { get; set; }
        public string VendorType { get; set; }

    }
}
