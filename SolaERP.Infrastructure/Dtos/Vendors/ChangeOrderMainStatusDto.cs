namespace SolaERP.Application.Dtos.Vendors;

public class ChangeOrderMainStatusDto
{
    public int OrderMainId { get; set; }
    public int Sequence { get; set; }
    public int ApproveStatusId { get; set; }
    public string Comment { get; set; }
    public int? RejectReasonId { get; set; }
}