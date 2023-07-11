namespace SolaERP.Application.Entities.ApproveStage
{
    public class ApproveStagesMain : BaseEntity
    {
        public int ApproveStageMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public string ApproveStageName { get; set; }
        public string ApproveStageCode { get; set; }
        public bool ReApproveOnChange { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StageCount { get; set; }
    }
}
