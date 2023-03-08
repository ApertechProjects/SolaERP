namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorPrequalification : BaseEntity
    {
        public int VendorPrequalificationId { get; set; }
        public int PrequalificationDesignId { get; set; }
        public int VendorId { get; set; }
        public string TextboxValue { get; set; }
        public string TextareaValue { get; set; }
        public bool CheckboxValue { get; set; }
        public bool RadioboxValue { get; set; }
        public int IntValue { get; set; }
        public decimal decimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public decimal Scoring { get; set; }
    }
}
