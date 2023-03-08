namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestMainDto
    {
        public Int64 RowNum { get; set; }
        public int RequestMainId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string RequestType { get; set; }
        public string RequestNo { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public string Buyer { get; set; }
        public int Requester { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public int Status { get; set; }
        public int ApproveStatus { get; set; }
        public string ApproveStatusName { get; set; }
        //public string CurrencyCode { get; set; }
    }
}
