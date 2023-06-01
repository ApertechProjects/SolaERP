using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class CompanyInformation
    {
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string CompanyAdress { get; set; }
        public string City { get; set; }
        public string WebSite { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool ISUserAgree { get; set; }

        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<Country> Countries { get; set; }
        public List<ProductService> Services { get; set; }
    }
}
