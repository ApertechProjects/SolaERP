using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services;

public class FixedAssetService : IFixedAssetService
{
    private readonly IFixedAssetRepository _fixedAssetRepository;
    private readonly IMapper _mapper;

    public FixedAssetService(IFixedAssetRepository fixedAssetRepository, IMapper mapper)
    {
        _fixedAssetRepository = fixedAssetRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<FixedAssetDto>>> GetAllAsync(int businessUnitId)
    {
        var entityList = await _fixedAssetRepository.GetAllAsync(businessUnitId);
        var resultList = _mapper.Map<List<FixedAssetDto>>(entityList);
        return ApiResponse<List<FixedAssetDto>>.Success(resultList, 200);
    }
}