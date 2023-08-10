namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class ValueEntity : BaseEntity
    {
        private DateTime? _dateTimeValue;
        public int VendorId { get; set; }
        public string TextboxValue { get; set; }
        public string TextareaValue { get; set; }
        public bool CheckboxValue { get; set; }
        public bool RadioboxValue { get; set; }
        public int IntValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime? DateTimeValue
        {
            get
            {
                if (_dateTimeValue?.Date == DateTime.MinValue)
                    _dateTimeValue = DateTime.Now;
                return _dateTimeValue;
            }
            set
            {
                _dateTimeValue = value;
            }
        }
        public decimal Scoring { get; set; }
    }
}
