namespace SolaERP.Application.Entities.Request
{
    public class RequestHeld
    {
        public long RowNum { get; set; }
        public int RequestMainId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string RequestType { get; set; }
        public string RequestNo { get; set; }
        public int Priority { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public int Requester { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public bool HasAttachments { get; set; }
    }
}