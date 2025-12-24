using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.WellRepair;

namespace SolaERP.Persistence.Services;

public class WellRepairService : IWellRepairService
{
    private readonly IWellRepairRepository _wellRepairRepository;

    public WellRepairService(IWellRepairRepository wellRepairRepository)
    {
        _wellRepairRepository = wellRepairRepository;
    }

    public async Task<ApiResponse<List<WellRepairListDto>>> GetWellRepairList()
    {
        var result = await _wellRepairRepository.GetWellRepairList();
        return ApiResponse<List<WellRepairListDto>>.Success(result);
    }
}











