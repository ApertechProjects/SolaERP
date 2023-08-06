using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Enums;
using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class RfqSaveCommandRequest
    {
        public int RFQMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public RfqType RFQType { get; set; }
        public string RFQNo { get; set; }
        public Emergency Emergency { get; set; }
        public DateTime? RFQDate { get; set; }
        public DateTime? RFQDeadline { get; set; }
        public string Buyer { get; set; }
        public Status Status { get; set; }
        public DateTime? RequiredOnSiteDate { get; set; }
        public DateTime? DesiredDeliveryDate { get; set; }
        public DateTime? SentDate { get; set; }
        public int SingleUnitPrice { get; set; }
        public ProcurementType ProcurementType { get; set; }
        public BiddingType BiddingType { get; set; }
        public List<int> SingleSourceReasonIds { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string Comment { get; set; }
        public string OtherReasons { get; set; }
        public int BusinessCategoryId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public List<RfqDetailSaveModel> RfqDetails { get; set; }
    }

    public class RfqSaveCommandResponse
    {
        public int Id { get; set; }
        public string RfqNo { get; set; }
    }
}
