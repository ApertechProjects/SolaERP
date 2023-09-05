using SolaERP.Application.Dtos.BusinessUnit;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class COBCDto
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public int BusinessUnitId { get; set; }
    }

    public class CodeOfBuConduct : BusinessUnitsAllDto
    {
        public string VendorFullName { get; set; }
        public int CobcID { get; set; }
        public int VendorId { get; set; }
        public bool IsAgreed { get; set; }
        public int Type { get; set; }
    }

}
