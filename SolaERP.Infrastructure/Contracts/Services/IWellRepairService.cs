using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.WellRepair;

namespace SolaERP.Application.Contracts.Services;

public interface IWellRepairService
{
    Task<ApiResponse<List<WellRepairListDto>>> GetWellRepairList();
    Task<ApiResponse<List<WellRepairLoadDto>>> LoadWellRepairs(int wellRepairId);
    Task<ApiResponse<List<WellCostListDto>>> GetWellCostList(int businessUnitId, DateTime dateFrom,  DateTime dateTo);
    Task<ApiResponse<List<AnalysisFromSunListDto>>> GetAnalysisListFromSun(int businessUnitId, int anlCatId);
    Task<ApiResponse<bool>> SaveWellRepairAsync(List<WellRepairRequest> data , int userId);
    Task<ApiResponse<bool>> SaveWellCostAsync(List<WellCostRequest> data, int userId);
}