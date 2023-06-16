namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class VendorBankDetail : BaseEntity
    {
        public int VendorBankDetailId { get; set; }
        public int VendorId { get; set; }
        public string Beneficiary { get; set; }
        public string BeneficiaryTaxId { get; set; }
        public string Address { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string SWIFT { get; set; }
        public string BankCode { get; set; }
        public string Currency { get; set; }
        public string BankTaxId { get; set; }
        public string CorrespondentAccount { get; set; }
    }
}
