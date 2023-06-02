namespace SolaERP.Application.Entities.ApproveStage
{
    public class ApproveStagesMain : BaseEntity
    {
        public int ApproveStageMainId { get; set; }
        public int ProcedureId { get; set; }
        public Procedure.Procedure Procedure { get; set; }
        public int BusinessUnitId { get; set; }
        public string ApproveStageCode { get; set; }
        public string ApproveStageName { get; set; }
        public bool ReApproveOnChange { get; set; }
    }
}
