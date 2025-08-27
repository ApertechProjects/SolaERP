using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.BarrelFlow;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;

namespace SolaERP.Persistence.Services;

public class BarrelFlowService : IBarrelFlowService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBarrelFlowRepository _barrelFlowRepository;

    public BarrelFlowService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBarrelFlowRepository barrelFlowRepository
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _barrelFlowRepository = barrelFlowRepository;
    }

    public async Task<ApiResponse<bool>> SaveBarrelFlowsAsync(
        List<BarrelFlowUIDDto> data)
    {
        var table = data.ConvertListOfCLassToDataTable();
        var result = await _barrelFlowRepository.SaveBarrelFlowsRegisterIUD(table);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true, 200);
    }

    public async Task<ApiResponse<List<BarrelFlowRegisterDto>>> BarrelFlowRegisterAsync(int businessUnitId,
        DateTime dateFrom, DateTime dateTo)
    {
        var result = await _barrelFlowRepository.GetBarrelFlowRegister(businessUnitId, dateFrom, dateTo);
        return ApiResponse<List<BarrelFlowRegisterDto>>.Success(result, 200);
    }
}