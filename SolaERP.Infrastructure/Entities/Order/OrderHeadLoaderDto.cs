using SolaERP.Application.Dtos.Attachment;
using System.Xml.Linq;

namespace SolaERP.Application.Entities.Order;

public class OrderHeadLoaderDto : BaseEntity
{
    private int? approveStageMainId;
    public int OrderMainId { get; set; }
    public int OrderTypeId { get; set; }
    public string OrderNo { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Emergency { get; set; }
    public DateTime? EntryDate { get; set; }
    public string EnteredBy { get; set; }
    public string ReasonCode { get; set; }
    public string ReasonName { get; set; }
    public string Buyer { get; set; }
    public string Comment { get; set; }
    public int? ApproveStageMainId
    {
        get
        {
            if (approveStageMainId == 0)
                approveStageMainId = null;
            return approveStageMainId;
        }
        set
        {
            approveStageMainId = value;
        }
    }
    public string VendorCode { get; set; }
    public string VendorName { get; set; }
    public string CompanyAddress { get; set; }
    public string ContactPerson { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string PhoneNo { get; set; }
    public string TaxId { get; set; }
    public string TaxCode { get; set; }
    public string Currency { get; set; }
    public int DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public int DeliveryTermId { get; set; }
    public string DeliveryTime { get; set; }
    public string PaymentTermId { get; set; }
    public decimal ExpectedCost { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? DesiredDeliveryDate { get; set; }
    public int RFQMainId { get; set; }
    public string RFQNo { get; set; }
    public DateTime? RFQDeadline { get; set; }
    public DateTime? RFQDate { get; set; }
    public DateTime? RequiredOnSiteDate { get; set; }
    public string ComparisonNo { get; set; }
    public DateTime? Comparisondeadline { get; set; }
    public DateTime? ComparisonDate { get; set; }
    public int BidMainId { get; set; }
    public int Status { get; set; }
    public int ApproveStatus { get; set; }
    public int BusinessUnitId { get; set; }
    public int GRNStatus { get; set; }
    public int InvoiceStatus { get; set; }
    public bool OrderPrint { get; set; }
    public string OrderNotes { get; set; }
    public int BudgetYear { get; set; }
    public string DestinationPoint { get; set; }
    public string KeyCode { get; set; }
    public string LCType { get; set; }

    public List<OrderDetailLoadDto> OrderDetails { get; set; }
    public List<AttachmentDto> Attachments { get; set; }
}