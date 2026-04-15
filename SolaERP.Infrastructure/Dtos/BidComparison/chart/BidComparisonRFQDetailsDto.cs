using SolaERP.Application.Dtos.Attachment;

namespace SolaERP.Application.Dtos.BidComparison;

public class BidComparisonRFQDetailsDto
{
    public int Id { get; set; }
    public int? LineNumber { get; set; }
    public string Description { get; set; }
    public string UomName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Budget { get; set; }
    public decimal RemainingBudget { get; set; }
    public int? RfqDetailId { get; set; }
    public bool? AlternativeItem { get; set; }
    public string RfqDetailDescription { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set;}
    public List<AttachmentViewDto> Attachments { get; set; }
    public bool HasAttachments { get; set; }
    public string TechnicalQuality { get; set; }
    
}