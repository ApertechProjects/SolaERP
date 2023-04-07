namespace SolaERP.Infrastructure.Entities.Request
{
    public class RequestAmendment : BaseEntity
    {
        public int RequestMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestNo { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public int UserId { get; set; }
        public int Requester { get; set; }
        public int Status { get; set; }
        public string SupplierCode { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public decimal LogisticTotal { get; set; }

    }
}
