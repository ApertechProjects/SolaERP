namespace SolaERP.Application.Dtos.Order;

public class OrderApprovalDto
{
    public int OrderApprovalId { get; set; }
    
    public string ApproveStageDetailsName { get; set; }
    
    public int Sequence { get; set; }
    
    public string FullName { get; set; }
    
    public string ApprovalStatusName { get; set; }
    
    public DateTime ApproveDate { get; set; }
    
    public string Comment { get; set; }
}