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
        public DateTime RequestDeadline { get; set; }
        public int UserId { get; set; }
        public int Requester { get; set; }
        public int Status { get; set; }
        public int Sequence { get; set; }//new
        public int ApproveStatus { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string SupplierCode { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public int RowNum { get; set; }
        public decimal LogisticsTotal { get; set; }
        public List<RequestDetailWithAnalysisCode> Details { get; set; }
    }
}
