namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class DueDiligenceValue : ValueEntity
    {
        public int VendorDueDiligenceId { get; set; }
        public int DueDiligenceDesignId { get; set; }
        public bool AgreementValue { get; set; }
    }
}
