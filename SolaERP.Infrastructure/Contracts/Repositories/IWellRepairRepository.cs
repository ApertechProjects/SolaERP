using SolaERP.Application.Dtos.WellRepair;

namespace SolaERP.Application.Contracts.Repositories;

public interface IWellRepairRepository
{
    Task<List<WellRepairListDto>> GetWellRepairList();
    Task<List<WellRepairLoadDto>> LoadWellRepairs(int wellRepairId);
    Task<List<WellCostListDto>> GetWellCostList(int businessUnitId, DateTime dateFrom, DateTime dateTo);
    Task<List<AnalysisFromSunListDto>> GetAnalysisListFromSun(int businessUnitId, int anlCatId);
    Task<bool> SaveWellRepairAsync(List<WellRepairRequest> data,  int userId);
    Task<bool> SaveWellCostAsync(List<WellCostRequest> dto,  int userId);
}