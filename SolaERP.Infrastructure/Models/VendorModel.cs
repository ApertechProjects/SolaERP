namespace SolaERP.Application.Models
{
    public class VendorModel
    {
        public int BusinessUnitId { get; set; } = 1;
        public string VendorName { get; set; }
        public string TaxId { get; set; }
        public string TaxOffice { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string PaymentTerms { get; set; }
        public int? CreditDays { get; set; }
        public int? _0DaysPayment { get; set; }
        public string Country { get; set; }
        public int? UserId { get; set; }
        public string OtherProducts { get; set; }
        public int? ApproveStageMainId { get; set; }
        public string CompanyAddress { get; set; }
        public DateTime CompanyRegistrationDate { get; set; }
    }
}
