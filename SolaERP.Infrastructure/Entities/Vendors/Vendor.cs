namespace SolaERP.Application.Entities.Vendors
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public int Buid { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string CompanyAdress { get; set; }
        public string City { get; set; }
        public string WebSite { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RepresentedCompanies { get; set; }
        public string RepresentedProducts { get; set; }
        public int CreditDays { get; set; }
        public string PaymentTerms { get; set; }
        public int AgreeWithDefaultDays { get; set; }
        public int PrequalificationCategoryId { get; set; }
        public int BusinessCategoryId { get; set; }
    }
}
