namespace SolaERP.Application.Entities.Request
{
    public class RequestTypes : BaseEntity
    {
        public int RequestTypeId { get; set; }
        public string RequestType { get; set; }
        public int BusinessUnitId { get; set; }
        public int ApproveStageMainId { get; set; }
    }
}
