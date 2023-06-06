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
        public int CobcID { get; set; }
        public int VendorId { get; set; }
    }

}
