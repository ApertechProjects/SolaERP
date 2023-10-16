namespace SolaERP.Application.Dtos.Request
{
    public class RequestTypesDto
    {
        public int RequestTypeId { get; set; }
        public string RequestType { get; set; }
        public int BusinessUnitId { get; set; }
        public int ApproveStageMainId { get; set; }
        public string KeyCode { get; set; }
    }
}
