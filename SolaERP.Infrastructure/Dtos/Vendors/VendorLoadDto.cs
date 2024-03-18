using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.SupplierEvaluation;

namespace SolaERP.Application.Dtos.Vendors
{
    public class VendorLoadDto
    {
        public int VendorId { get; set; }
        public int RevisionVendorId { get; set; }
        public int BlackList { get; set; }
        public string BlackListDescription { get; set; }
        public decimal Rating { get; set; }
        public int ReviseNo { get; set; }
        public DateTime? ReviseDate { get; set; }
        public int ApproveStatus { get; set; }
        public int Status { get; set; }
        public string VendorCode { get; set; }
        public string TaxId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Postal_ZIP { get; set; }
        public string EmailAddress { get; set; }
        public string DefaultCurrency { get; set; }
        public string Website { get; set; }
        public string Address2 { get; set; }
        public string Phone_Mobile { get; set; }
        public string ContactPerson { get; set; }
        public int? ShipVia { get; set; }
        public int? DeliveryTerms { get; set; }
        public string PaymentTerms { get; set; }
        public int? WithHoldingTaxId { get; set; }
        public int? Tax { get; set; }
        public string RepresentedProducts { get; set; }
        public string RepresentedCompanies { get; set; }
        public int CreditDays { get; set; }
        public bool _60DaysPayment { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string OtherProducts { get; set; }
        public DateTime? CompanyRegistrationDate { get; set; }
        public string TaxOffice { get; set; }
        public string Logo { get; set; }
        public List<VendorBankDetailDto> BankAccounts { get; set; }
        public AttachmentDto AttachmentLogo { get; set; }
    }
}