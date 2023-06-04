namespace SolaERP.Application.Entities.ApproveStage
{
    public class ApproveStagesMain : BaseEntity
    {
        public int ApproveStageMainId { get; set; }
        public int ProcedureId { get; set; }
        public object ProcedureName { get; set; }
        public object ApproveStageName { get; set; }
        public object ApproveStageCode { get; set; }
        public bool ReApproveOnChange { get; set; }
        public object CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StageCount { get; set; }
    }
}
