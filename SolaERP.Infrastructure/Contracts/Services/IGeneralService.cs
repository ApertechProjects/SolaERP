using SolaERP.Application.Dtos.General;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Status;
using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.Contracts.Services
{
    public interface IGeneralService
    {
        Task<ApiResponse<List<BusinessCategory>>> BusinessCategories();
        Task<ApiResponse<List<StatusDto>>> GetStatus();
        Task<ApiResponse<List<RejectReasonDto>>> RejectReasonsForInvoice();
        Task<ApiResponse<List<RejectReasonDto>>> RejectReasons();
        Task<string> ReasonCode(int rejectReasonId);
        Task<ApiResponse<BaseAndReportCurrencyRate>> GetBaseAndReportCurrencyRateAsync(
            DateTime date,
            string currency,
            int businessUnitId);

        Task<bool> DailyCurrencyIsExist(
           DateTime date,
           string currency,
           int businessUnitId);

        Task<RejectReasonDto> GetRejectReasonByCode(string rejectReasonId);
    }
}