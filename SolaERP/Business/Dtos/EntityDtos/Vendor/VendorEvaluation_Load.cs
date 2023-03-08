using SolaERP.Business.Dtos.EntityDtos.Attachment;

namespace SolaERP.Business.Dtos.EntityDtos.Vendor
{
    public partial class VendorEvaluation_Load
    {
        public System.Int32 VendorEvaluationFormId { get; set; }
        public System.Int32 VendorId { get; set; }
        public System.Int32 ContextOfTheOrganization1 { get; set; }
        public System.Int32 ContextOfTheOrganization2 { get; set; }
        public System.Int32 ContextOfTheOrganization3 { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public System.Int32 Leadership1 { get; set; }
        public System.Int32 Leadership2 { get; set; }
        public System.Int32 Planning1 { get; set; }
        public System.Int32 Planning2 { get; set; }
        public System.Int32 Planning3 { get; set; }
    }

    public partial class VendorEvaluation_Load
    {
        public List<AttachmentList_Load> ISO { get; set; }
        public List<AttachmentList_Load> OTH { get; set; }

    }
}
