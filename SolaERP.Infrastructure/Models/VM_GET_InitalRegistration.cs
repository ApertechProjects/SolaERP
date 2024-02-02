using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Models
{
    public class VM_GET_InitalRegistration
    {
        public CompanyInfoViewDto CompanyInformation { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<BusinessSector> BusinessSectors { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<ProductService> Services { get; set; }
        public List<Country> Countries { get; set; }

    }
}
