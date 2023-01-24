namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestMainWithDetailsDto
    {
        public int RequestMainID { get; set; }
        public string BusinessUnitCode { get; set; }
        public int RowNum { get; set; }
        public string RequestType { get; set; }
        public string RequestNo { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public string Buyer { get; set; }
        public string Requester { get; set; }
        public string RequestCommand { get; set; }
        public string OpertorComment { get; set; }
        public string QualityRequired { get; set; }
        public int ApproveStatus { get; set; }
        public List<RequestDetailDto> Details { get; set; }
    }
}
