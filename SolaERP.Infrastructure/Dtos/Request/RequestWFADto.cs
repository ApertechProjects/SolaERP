namespace SolaERP.Application.Dtos.Request
{
    public class RequestWFADto
    {
        public Int64 RowNum { get; set; }
        public int RequestMainId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string RequestType { get; set; }
        public string RequestNo { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public int UserId { get; set; }
        public int Requester { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string ApproveStatus { get; set; }
        public string ApproveStatusName { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public string SupplierCode { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public decimal LogisticsTotal { get; set; }
        public List<RequestDetailDto> RequestDetailDtos { get; set; }
    }
}
