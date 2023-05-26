using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class CompanyInformation
    {
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<Country> Countries { get; set; }
        public List<ProductService> Services { get; set; }
    }
}
