using SolaERP.Business.Dtos.EntityDtos.Attachment;
using SolaERP.Business.Dtos.EntityDtos.General;

namespace SolaERP.Business.Dtos.EntityDtos.Vendor
{
    public partial class Vendor_Load
    {
        public System.Int32 VendorId { get; set; }
        public System.String VendorCode { get; set; }
        public System.String VendorName { get; set; }
        public System.String TaxId { get; set; }
        public System.String CompanyLocation { get; set; }
        public System.String CompanyWebsite { get; set; }
        public System.String RepresentedProducts { get; set; }
        public System.String RepresentedCompanies { get; set; }
        public System.String PaymentTermsCode { get; set; }
        public System.Int32 CreditDays { get; set; }
        public System.Int32 AgreeWithDefaultDays { get; set; }
        public System.String CountryCode { get; set; }
        public System.Int32 StatusId { get; set; }
        public System.String StatusName { get; set; }
        public System.Int32 Id { get; set; }
        public System.String FullName { get; set; }
        public System.Int32 StatusId1 { get; set; }
        public System.String StatusName1 { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public System.Int32 Sessions { get; set; }
        public System.DateTime LastActivity { get; set; }
        public System.String UserName { get; set; }
        public System.String Position { get; set; }
        public System.Int32 BankId { get; set; }
        public System.String BeneficiaryBankName { get; set; }
        public System.String CurrencyCode { get; set; }
        public System.String BankAccountNumber { get; set; }
        public System.String BankAddress { get; set; }
        public System.String BankAddress1 { get; set; }
        public System.String BeneficiaryFullName { get; set; }
        public System.String BeneficiaryAddress { get; set; }
        public System.String BeneficiaryAddress1 { get; set; }
        public System.String BeneficiaryBankCode { get; set; }
        public System.String IntermediaryBankCodeNumber { get; set; }
        public System.String IntermediaryBankCodeType { get; set; }
        public System.String CompanyAddress { get; set; }
        public System.DateTime CompanyRegistrationDate { get; set; }
        public System.Int32 PrequalificationCategoryId { get; set; }
        public System.Int32 VendorDueDiligenceId { get; set; }
        public System.Int32 LineNo { get; set; }
        public System.Int32 DueDiligenceDesignId { get; set; }
        public System.Int32 VendorId1 { get; set; }
        public System.String TextboxValue { get; set; }
        public System.Boolean CheckboxValue { get; set; }
        public System.Boolean RadioboxValue { get; set; }
        public System.Int32 IntValue { get; set; }
        public System.Decimal DecimalValue { get; set; }
        public System.DateTime DateTimeValue { get; set; }
        public System.Decimal Scoring { get; set; }
        public System.Int32 Weight { get; set; }
        public System.Int32 VendorEvaluationFormId { get; set; }
        public System.Int32 VendorId2 { get; set; }
        public System.Int32 ContextOfTheOrganization1 { get; set; }
        public System.Int32 ContextOfTheOrganization2 { get; set; }
        public System.Int32 ContextOfTheOrganization3 { get; set; }
        public System.DateTime ExpirationDate1 { get; set; }
        public System.Int32 Leadership1 { get; set; }
        public System.Int32 Leadership2 { get; set; }
        public System.Int32 Planning1 { get; set; }
        public System.Int32 Planning2 { get; set; }
        public System.Int32 Planning3 { get; set; }
        public System.Int32 ApprovalId { get; set; }
        public System.Int32 Sequence { get; set; }
        public System.Int32 ApproveStageDetailsId { get; set; }
        public System.Int32 UserId { get; set; }
        public System.Int32 ApproveStatusId { get; set; }
        public System.DateTime ApproveDate { get; set; }
        public System.String Comment { get; set; }
        public System.Int32 VendorPrequalificationId { get; set; }
        public System.Int32 PrequalificationDesignId { get; set; }
        public System.Int32 VendorId3 { get; set; }
        public System.String TextboxValue1 { get; set; }
        public System.String TextareaValue { get; set; }
        public System.Boolean CheckboxValue1 { get; set; }
        public System.Boolean RadioboxValue1 { get; set; }
        public System.Int32 IntValue1 { get; set; }
        public System.Decimal DecimalValue1 { get; set; }
        public System.DateTime DateTimeValue1 { get; set; }
        public System.Decimal Scoring1 { get; set; }
    }

    public partial class Vendor_Load
    {
        public List<AttachmentList_Load> LOGO { get; set; }
        public List<AttachmentList_Load> OLET { get; set; }
        public List<VendorProductServiceCls> VendorProductServices { get; set; }
        public List<PrequalificationCategory>  PrequalificationCategories{ get; set; }
        public List<int>  PrequalificationCategorieIds{ get; set; }
        public List<int> VendorProductServiceIds { get; set; }
        public string SelectedVendorProductServices { get; set; }
        public string SelectedPrequalificationCategories { get; set; }

    }
}
