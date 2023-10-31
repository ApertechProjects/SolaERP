namespace SolaERP.Application.Dtos.Vendors;

public class VendorRFQListResponseDto
{
    public int BidMainId { get; set; }
    public int RFQMainId { get; set; }
    public long LineNo { get; set; }
    public string ParticipationStatus { get; set; }
    public string Emergency { get; set; }
    public string RFQStatus { get; set; }
    public string RFQNo { get; set; }
    public string BusinessCategoryId { get; set; }
    public string RFQType { get; set; }
    public int BiddingType { get; set; }
    public DateTime DesiredDeliveryDate { get; set; }
    public DateTime RFQDate { get; set; }
    public DateTime RFQDeadline { get; set; }
    public DateTime RespondedDate { get; set; }
    public string EnteredBy { get; set; }
    public DateTime SentDate { get; set; }
    public DateTime CreatedDate { get; set; }
}