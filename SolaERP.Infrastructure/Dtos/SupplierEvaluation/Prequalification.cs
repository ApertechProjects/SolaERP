namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class PrequalificationDto
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string TextboxValue { get; set; }
        public string TextareaValue { get; set; }
        public bool CheckboxValue { get; set; }
        public bool RadioboxValue { get; set; }
        public int IntValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public decimal Scoring { get; set; }
    }
}
