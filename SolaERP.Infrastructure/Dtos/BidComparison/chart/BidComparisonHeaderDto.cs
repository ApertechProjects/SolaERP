namespace SolaERP.Application.Dtos.BidComparison;

public class BidComparisonHeaderDto
{
    public int BidComparisonId { get; set; }
    public int RFQMainId { get; set; }
    public int BusinessUnitId { get; set; }
    public string RFQNo { get; set; }
    public int BiddingTypeId { get; set; }
    public string ProcurementType { get; set; }
    public DateTime? RFQDate { get; set; }
    public DateTime? SentDate { get; set; }
    public string OtherReasons { get; set; }
    public DateTime? RFQDeadline { get; set; }
    public string CreatedBy { get; set; }
    public int CreatedById { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string ComparisonNo { get; set; }
    public DateTime? ComparisonDate { get; set; }
    public DateTime? ComparisonDeadline { get; set; }
    public DateTime? RequiredOnSiteDate { get; set; }
    public string BuyerId { get; set; }
    public string BuyerName { get; set; }
    public int ApproveStageMainId { get; set; }
    public int StatusId { get; set; }
    public int approveStatusId { get; set; }
    public string SpecialistComment { get; set; }
    public string SingleSourceReasons { get; set; }
    public string RFQSingleSourceReasons { get; set; }
    public string Attachments { get; set; }
    public string ProcurementMethodId { get; set; }
    public string ProcurementMethodName { get; set; }
    public string RFQSingleSourceAttachments { get; set; }
    public string FinalProtocol { get; set; }
    public DateTime? FinalProtocolDate { get; set; }
    public string RFQComment { get; set; }
    public DateTime? ExtendedDeadline { get; set; }
    public string RebiddingRejectReasonId { get; set; }
    public bool OpenRFQ { get; set; }
    public string RejectReasonId { get; set; }
    public int RFQMainStatusId { get; set; }
    
}