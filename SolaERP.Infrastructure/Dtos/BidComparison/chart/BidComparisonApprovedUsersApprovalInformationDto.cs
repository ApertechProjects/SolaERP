namespace SolaERP.Application.Dtos.BidComparison;

public class BidComparisonApprovedUsersApprovalInformationDto
{
    public int? userId { get; set; }
    public string fullName { get; set; }
    public DateTime? approveDate { get; set; }
    public string comment { get; set; }

}