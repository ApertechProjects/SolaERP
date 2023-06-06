using SolaERP.Application.Dtos.BusinessUnit;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class NDADto
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public int VendorId { get; set; }
    }

    public class NonDisclosureAgreement : BusinessUnitsAllDto
    {
        public int NdaID { get; set; }
        public int VendorId { get; set; }
    }

}
