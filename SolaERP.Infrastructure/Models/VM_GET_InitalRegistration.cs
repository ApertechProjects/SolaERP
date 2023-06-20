using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class VM_GET_InitalRegistration
    {
        public ContactPersonDto ContactPerson { get; set; }
        public CompanyInfoDto CompanyInformation { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<PrequalificationCategory> PrequalificationTypes { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<ProductService> Services { get; set; }
        public List<Country> Countries { get; set; }

    }
}
