namespace SolaERP.Application.Entities.Invoice;

public class MatchingMain : BaseEntity
{
    public int OrderMainId { get; set; }
    public string OrderNo { get; set; }
    public string VendorCode { get; set; }
    public string VendorName { get; set; }
    public string Currency { get; set; }
}