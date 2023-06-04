using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class VM_GET_InitalRegistration
    {
        public ContactPersonDto ContactPerson { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
    }
}
