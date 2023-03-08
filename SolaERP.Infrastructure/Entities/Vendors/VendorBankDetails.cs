namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorBankDetails : BaseEntity
    {
        public int VendorBankDetailId { get; set; }
        public int VendorId { get; set; }
        public string BankName { get; set; }
        public string CurrencyCode { get; set; }
        public string IBAN { get; set; }
        public string BankAddress { get; set; }
        public string BankAddress1 { get; set; }
        public string BeneficiarName { get; set; }
        public string BeneficiarAddress { get; set; }
        public string BeneficiarAddress1 { get; set; }
        public string BeneficiarBankdCode { get; set; }
        public string IntermediaryBankCode { get; set; }
        public string IntermediaryBankCodeType { get; set; }
    }
}
