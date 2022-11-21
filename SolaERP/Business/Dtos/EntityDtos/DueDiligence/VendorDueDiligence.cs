namespace SolaERP.Business.Dtos.EntityDtos.DueDiligence
{
    public class VendorDueDiligence
    {
        public System.Int32 VendorDueDiligenceId { get; set; }
        public System.Int32 DueDiligenceDesignId { get; set; }
        public System.Int32 VendorId { get; set; }
        public System.String TextboxValue { get; set; }
        public System.String TextareaValue { get; set; }
        public System.Boolean CheckboxValue { get; set; }
        public System.Boolean RadioboxValue { get; set; }
        public System.Int32 IntValue { get; set; }
        public System.Decimal DecimalValue { get; set; }
        public System.DateTime DateTimeValue { get; set; }
        public System.Decimal Scoring { get; set; }
    }
}
