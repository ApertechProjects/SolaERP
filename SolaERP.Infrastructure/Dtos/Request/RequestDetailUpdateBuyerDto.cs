namespace SolaERP.Application.Dtos.Request;

public class RequestDetailUpdateBuyerDto
{
    public int RequestDetailId { get; set; }
    public int BusinessUnitId { get; set; }
    public string Buyer { get; set; }
}