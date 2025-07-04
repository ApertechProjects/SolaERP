using SolaERP.Application.Dtos.Request;

namespace SolaERP.Application.Entities.Request
{
    public class RequestCardMain : BaseEntity
    {
        public int RequestMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string Buyer { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public int RequestTypeId { get; set; }
        public string KeyCode { get; set; }
        public string RequestNo { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Now.Date;
        public DateTime RequestDate { get; set; } = DateTime.Now.Date;
        public DateTime RequestDeadline { get; set; } = DateTime.Now.AddDays(7);
        public int UserId { get; set; }
        public int Requester { get; set; }
        public int Status { get; set; }
        public int Destination { get; set; }
        public string ApproveStatus { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public decimal LogisticsTotal { get; set; }
        public string PotentialVendor { get; set; }
        public int Priority { get; set; }
        public int ApproveStageMainId { get; set; }
        public string Location { get; set; }
        public string Warehouse { get; set; }
        public List<RequestCardDetail> requestCardDetails { get; set; }
        public List<RequestCardAnalysis> requestCardAnalysis { get; set; }
    }
}