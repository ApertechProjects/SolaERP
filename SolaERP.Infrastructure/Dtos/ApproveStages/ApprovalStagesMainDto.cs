namespace SolaERP.Application.Dtos.ApproveStage
{
    public class ApprovalStagesMainDto : ApproveStages.ApprovalStagesDto
    {
        public int ProcedureId { get; set; }
        public int BusinessUnitId { get; set; }
        public string ProcedureName { get; set; }
        public string ApproveStageCode { get; set; }
        public bool ReApproveOnChange { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StageCount { get; set; }
    }
}
