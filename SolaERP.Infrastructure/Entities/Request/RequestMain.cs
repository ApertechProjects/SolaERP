namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestMain : BaseEntity
    {
        public int RequestMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestNo { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequsetDeadline { get; set; }
        public int UserId { get; set; }
        public int Requester { get; set; }
        public int Status { get; set; }
        public string SupplierCode { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public int RowNum { get; set; }
        public string Buyer { get; set; }
        public decimal LogisticTotal { get; set; }
        public string ApproveStatus { get; set; }

        public RequestDetail Deta { get; set; }
    }
}
