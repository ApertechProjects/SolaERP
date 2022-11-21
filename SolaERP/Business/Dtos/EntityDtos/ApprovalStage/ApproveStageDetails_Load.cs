namespace SolaERP.Business.Dtos.EntityDtos.ApprovalStage
{
    public partial class ApproveStageDetails_Load
    {
        public System.Int32 ApproveStageDetailsId { get; set; }
        public System.Int32 ApproveStageMainId { get; set; }
        public System.String ApproveStageDetailsName { get; set; }
        public System.Int32 Sequence { get; set; }
    }

    public partial class ApproveStageDetails_Load
    {
        public List<ApprovalStageRoles_Load> approvalStageRoles_Loads { get; set; }
        public string Type { get; set; }
    }
}
