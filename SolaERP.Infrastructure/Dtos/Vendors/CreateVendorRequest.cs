namespace SolaERP.Application.Dtos.Payment;

public class CreateVendorRequest
{
    public string VendorCode { get; set; }
    public int BusinessUnitId { get; set; }
    public int UserId { get; set; }
}