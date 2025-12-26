using SolaERP.Application.Models;

namespace SolaERP.Application.Dtos.BidComparison;

public class BidComparisonAttachmentCardDto
{
    public int BidComparisonMainId { get; set; }
    public List<AttachmentSaveModel> Attachments { get; set; }

}