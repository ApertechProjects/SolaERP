namespace SolaERP.Application.Dtos.Vendors;

public class VendorRFQListDto
{
    public int BidMainId { get; set; }
    public int RFQMainId { get; set; }
    public long LineNo { get; set; }
    public string ParticipationStatus { get; set; }
    public int Emergency { get; set; }
    public int RFQStatus { get; set; }
    public string RFQNo { get; set; }
    public int BusinessCategoryId { get; set; }
    public string BusinessCategoryName { get; set; }
    public int RFQType { get; set; }
    public DateTime DesiredDeliveryDate { get; set; }
    public DateTime RFQDate { get; set; }
    public DateTime RFQDeadline { get; set; }
    public DateTime RespondedDate { get; set; }
    public string EnteredBy { get; set; }
    public DateTime SentDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string BiddingType { get; set; }
}