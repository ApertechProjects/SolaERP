using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.WellRepair;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services;

public class WellRepairService : IWellRepairService
{
    private readonly IWellRepairRepository _wellRepairRepository;
    private readonly IUnitOfWork _unitOfWork;
    public WellRepairService(IWellRepairRepository wellRepairRepository, IUnitOfWork unitOfWork)
    {
        _wellRepairRepository = wellRepairRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<List<WellRepairListDto>>> GetWellRepairList()
    {
        var result = await _wellRepairRepository.GetWellRepairList();
        return ApiResponse<List<WellRepairListDto>>.Success(result);
    }
    
    public async Task<ApiResponse<List<WellRepairLoadDto>>> LoadWellRepairs(int wellRepairId)
    {
        var result = await _wellRepairRepository.LoadWellRepairs(wellRepairId);
        return ApiResponse<List<WellRepairLoadDto>>.Success(result);
    }
    public async Task<ApiResponse<List<WellCostListDto>>> GetWellCostList(int businessUnitId, DateTime dateFrom,  DateTime dateTo)
    {
        var result = await _wellRepairRepository.GetWellCostList(businessUnitId, dateFrom, dateTo);
        return ApiResponse<List<WellCostListDto>>.Success(result);
    }
    
    public async Task<ApiResponse<List<AnalysisFromSunListDto>>> GetAnalysisListFromSun(int businessUnitId, int anlCatId)
    {
        var result = await _wellRepairRepository.GetAnalysisListFromSun(businessUnitId, anlCatId);
        return ApiResponse<List<AnalysisFromSunListDto>>.Success(result);
    }

    public async Task<ApiResponse<bool>> SaveWellRepairAsync(WellRepairRequest data, int userId)
    {
        await _wellRepairRepository.SaveWellRepairAsync(data, userId);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true);
    } 
    public async Task<ApiResponse<bool>> SaveWellCostAsync(WellCostRequest data,  int userId)
    {
        await _wellRepairRepository.SaveWellCostAsync(data, userId);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true);
    } 
}











