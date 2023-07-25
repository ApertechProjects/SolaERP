namespace SolaERP.Application.Entities.Vendors
{
    public class Vendor : BaseEntity
    {
        public int VendorId { get; set; }
        public int Buid { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string CompanyAdress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string WebSite { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RepresentedCompanies { get; set; }
        public string RepresentedProducts { get; set; }
        public int CreditDays { get; set; }
        public string PaymentTerms { get; set; }
        public int AgreeWithDefaultDays { get; set; }
        public int PrequalificationCategoryId { get; set; }
        public int BusinessCategoryId { get; set; }

        public decimal? Rating { get; set; }
        public int? BlackList { get; set; }
        public string BlackListDescription { get; set; }
        public int? ReviseNo { get; set; }
        public DateTime? ReviseDate { get; set; }
        public string Description { get; set; }
        public string Address2 { get; set; }
        public string DefaultCurrency { get; set; }
        public string Postal { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public int? ShipmentId { get; set; }
        public int? DeliveryTermId { get; set; }
        public string OtherProducts { get; set; }
        public int? WithHoldingTaxId { get; set; }
        public int? TaxesId { get; set; }
        public bool IsNewVendor() => VendorId <= 0;


    }
}
