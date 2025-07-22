namespace SolaERP.Application.Entities.Request;

public class WarehouseInfo : BaseEntity
{
    public string WHSE_CODE { get; set; }
    public string WarehouseName { get; set; }
    public string TelephoneNo { get; set; }
    public string TelephoneWAPP { get; set; }
    public string Address1 { get; set; }
    public string Email { get; set; }
    public string ContactPerson { get; set; }
}