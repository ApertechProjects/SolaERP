using SolaERP.Business.Dtos.EntityDtos.Attachment;

namespace SolaERP.Business.Dtos.EntityDtos.DueDiligence
{
    public partial class DueDiligence
    {
        public System.Int32 DueDiligenceDesignId { get; set; }
        public System.Int32 LineNo { get; set; }
        public System.String Label { get; set; } = "";
        public System.Boolean HasTextbox { get; set; }
        public System.Boolean HasTextarea { get; set; }
        public System.Boolean HasCheckbox { get; set; }
        public System.Boolean HasRadiobox { get; set; }
        public System.Boolean HasInt { get; set; }
        public System.Boolean HasDecimal { get; set; }
        public System.Boolean HasDateTime { get; set; }
        public System.Boolean HasAttachment { get; set; }
        public System.Boolean HasBankList { get; set; }
        public System.String Title { get; set; } = "";
        public System.Int32 Weight { get; set; }
        public System.Boolean ParentCompanies { get; set; }
    }

    public partial class DueDiligence
    {
        public string PercentWeight { get; set; }
        public VendorDueDiligence Values { get; set; }
        public List<AttachmentList_Load> Attachments { get; set; }
    }
}
