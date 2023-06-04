using SolaERP.Application.Entities;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class CompanyInformation
    {
        public CompanyInfoDto CompanyInfo { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<Country> Countries { get; set; }
        public List<ProductService> Services { get; set; }
    }

    public class CompanyInfoDto
    {
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string CompanyAdress { get; set; }
        public string City { get; set; }
        public string WebSite { get; set; }
        public DateTime RegistrationDate { get; set; }
    }


    public class CompanyInfo : BaseEntity
    {
        public string VendorName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string Location { get; set; }
        public object Country { get; set; }
        public string Website { get; set; }
        public DateTime CompanyRegistrationDate { get; set; }
    }
}
