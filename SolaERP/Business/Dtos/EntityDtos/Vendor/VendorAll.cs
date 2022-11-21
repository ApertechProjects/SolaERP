namespace SolaERP.Business.Dtos.EntityDtos.Vendor
{
    public class VendorAll
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TaxId { get; set; }
        public string CompanyLocation { get; set; }
        public string CompanyWebsite { get; set; }
        public string RepresentedProducts { get; set; }
        public string RepresentedCompanies { get; set; }
        public string PaymentTermsCode { get; set; }
        public int CreditDays { get; set; }
        public int AgreeWithDefaultDays { get; set; }
        public string CountryCode { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string ApproveStatusName { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public int StatusId1 { get; set; }
        public string StatusName1 { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Sessions { get; set; }
        public DateTime LastActivity { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
    }
}
