using SolaERP.Infrastructure.Dtos.Request;

namespace SolaERP.Infrastructure.Models
{
    public class RequestSaveModel
    {
        public int RequestMainId { get; set; }
        public int RequestTypeId { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public string SupplierCode { get; set; }
        public int Requester { get; set; }
        public int Status { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public decimal LogisticTotal { get; set; }
        public List<RequestDetailDto> Details { get; set; }
    }
}
