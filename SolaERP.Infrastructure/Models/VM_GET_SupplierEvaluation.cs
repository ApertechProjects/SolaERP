using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.SupplierEvaluation;

namespace SolaERP.Application.Models
{
    public class VM_GET_SupplierEvaluation
    {
        public List<BusinessUnitsDto> BusinessUnits { get; set; }
        public CompanyInformation CompanyInformation { get; set; }
        public BankCodesDto BankCodes { get; set; }
        public List<DueDiligenceDesignDto> DueDiligenceDesign { get; set; }
    }
}
