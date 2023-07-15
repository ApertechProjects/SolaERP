namespace SolaERP.Application.Dtos.ApproveStage
{
    public class ApproveStagesMainDto
    {
        public int Id { get; set; }
        public int ProcedureId { get; set; }
        public int BusinessUnitId { get; set; }
        public object ProcedureName { get; set; }
        public object ApproveStageName { get; set; }
        public object ApproveStageCode { get; set; }
        public bool ReApproveOnChange { get; set; }
        public object CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StageCount { get; set; }
    }
}
