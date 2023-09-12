namespace SolaERP.Application.Entities.Vendors;

public class VendorRFQStatusChangeRequest
{
    public int RFQMainId { get; set; }
    public int Status { get; set; }
    public string VendorCode { get; set; }
}