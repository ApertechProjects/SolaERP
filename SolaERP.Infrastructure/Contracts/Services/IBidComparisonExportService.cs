using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.BidComparison;

namespace SolaERP.Application.Contracts.Services;

public interface IBidComparisonExportService
{
    Task GetCardExportByRfqMainIdAsync(
        int rfqMainId,
        HttpResponse response,
        string logoLink,
        List<BidComparisonBidHeaderDto> bids,
        List<BidComparisonRFQDetailsDto> rfqDetails,
        BidComparisonHeaderDto bcc,
        List<string> requestDepartmentCodes,
        List<BidComparisonApprovedUsersApprovalInformationDto> approvedUsers,
        List<string> requestNumbers);
}
