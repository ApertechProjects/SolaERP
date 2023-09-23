using SolaERP.Application.Dtos.Attachment;
using SolaERP.Application.Dtos.Request;

namespace SolaERP.Application.Models
{
    public class RequestSaveModel
    {
        public int RequestMainId { get; set; }
        public int RequestTypeId { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestDeadline { get; set; }
        public int Requester { get; set; }
        public int Status { get; set; }
        public string RequestComment { get; set; }
        public string OperatorComment { get; set; }
        public string PotentialVendor { get; set; }

        public string QualityRequired { get; set; }
        public string CurrencyCode { get; set; }
        public decimal LogisticsTotal { get; set; }
        public string Buyer { get; set; }
        public int Destination { get; set; }
        public int Priority { get; set; }
        public int ApproveStageMainId { get; set; }
        public string RequestNo { get; set; }
        public List<RequestDetailDto> Details { get; set; }
        public List<AttachmentSaveModel> Attachments { get; set; }
    }
}
