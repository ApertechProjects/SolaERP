namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestMainDto
    {
        public int RequestMainID { get; set; }
        public string BusinessUnitCode { get; set; }
        public int RowNum { get; set; }
        public string RequestType { get; set; }
        public string RequestNo { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
    }
}
