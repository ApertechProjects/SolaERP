namespace SolaERP.Application.Dtos.Request
{
    public class RequestAmendmentDto
    {
        public int RequestMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestNo { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public int UserId { get; set; }
        public int Requester { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string SupplierCode { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public decimal LogisticsTotal { get; set; }
    }
}
