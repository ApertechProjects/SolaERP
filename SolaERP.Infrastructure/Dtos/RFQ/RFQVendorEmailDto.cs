namespace SolaERP.Application.Dtos.RFQ;

public class RFQVendorEmailDto
{
    public int RFQMainId { get; set; }
    public string VendorCode { get; set; }
    public string VendorName { get; set; }
    public DateTime RfqDeadline { get; set; }
    public string VendorEmail { get; set; }
    public int BusinessUnitId { get; set; }
    public string BusinessUnitName { get; set; }
    public string RFQNo { get; set; }
    
    public string BidNo { get; set; }
}