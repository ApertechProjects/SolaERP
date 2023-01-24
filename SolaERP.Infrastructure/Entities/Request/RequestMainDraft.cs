namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestMainDraft : BaseEntity
    {
        public int RequestMainId { get; set; }
        public int Status { get; set; }
        public string BusinessUnitCode { get; set; }
        public int RowNum { get; set; }
        public string RequestType { get; set; }
        public int RequetsNo { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public string Buyer { get; set; }
        public string Requester { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string ApproveStatus { get; set; }
    }
}
