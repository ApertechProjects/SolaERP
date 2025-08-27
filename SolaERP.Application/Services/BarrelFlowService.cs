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

    public async Task<ApiResponse<List<BarrelFlowRegisterDto>>> GetBarrelFlowRegisterAsync(int businessUnitId,
        DateTime dateFrom, DateTime dateTo)
    {
        var result = await _barrelFlowRepository.GetBarrelFlowRegister(businessUnitId, dateFrom, dateTo);
        return ApiResponse<List<BarrelFlowRegisterDto>>.Success(result, 200);
    }

    public async Task<ApiResponse<List<BarrelFlowPeriodConvertListDto>>> GetPeriodListByBusinessIdAsync(
        int businessUnitId)
    {
        var data = await _barrelFlowRepository.GetPeriodListByBusinessId(businessUnitId);

        List<BarrelFlowPeriodConvertListDto> result = new List<BarrelFlowPeriodConvertListDto>();

        foreach (var d in data)
        {
            var periodFrom = Int32.Parse(d.PeriodFrom);
            var periodTo = Int32.Parse(d.PeriodTo);

            for (int i = periodFrom; i <= periodTo; i++)
            {
                if (i % 100 == 13)
                {
                    var nextYear = ((i / 1000) + 1);
                    // Novbeti il 2024012
                    i = Int32.Parse(nextYear.ToString() + "000");
                    continue;
                }

                BarrelFlowPeriodConvertListDto dto = new BarrelFlowPeriodConvertListDto();
                dto.name = i.ToString();
                result.Add(dto);
            }
        }

        return ApiResponse<List<BarrelFlowPeriodConvertListDto>>.Success(result, 200);
    }

    public async Task<ApiResponse<bool>> SaveBarrelFlowBudgetRegisterAsync(List<BarrelFlowBudgetUIDDto> data)
    {
        var table = data.ConvertListOfCLassToDataTable();
        var result = await _barrelFlowRepository.SaveBarrelFlowBudgetRegisterIUD(table);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true, 200);
    }

    public async Task<ApiResponse<List<BarrelFlowBudgetRegisterDto>>> GetBarrelFlowBudgetRegisterAsync(
        int businessUnitId,
        DateTime dateFrom, DateTime dateTo)
    {
        var result = await _barrelFlowRepository.GetBarrelFlowBudgetRegister(businessUnitId, dateFrom, dateTo);
        return ApiResponse<List<BarrelFlowBudgetRegisterDto>>.Success(result, 200);
    }

    public async Task<ApiResponse<bool>> SaveProductionRevenueRegisterAsync(List<ProductionRevenueRegisterIUDDto> data)
    {
        var table = data.ConvertListOfCLassToDataTable();
        var result = await _barrelFlowRepository.SaveProductionRevenueRegisterIUD(table);
        await _unitOfWork.SaveChangesAsync();
        return ApiResponse<bool>.Success(true, 200);
    }

    public async Task<ApiResponse<List<ProductionRevenueListDto>>> GetProductionRevenueRegisterAsync(
        int businessUnitId,
        DateTime dateFrom, DateTime dateTo)
    {
        var result = await _barrelFlowRepository.GetProductionRevenueRegister(businessUnitId, dateFrom, dateTo);
        return ApiResponse<List<ProductionRevenueListDto>>.Success(result, 200);
    }

    public async Task<ApiResponse<List<FactForecastListDto>>> GetFactForecastListAsync()
    {
        var result = await _barrelFlowRepository.GetFactForecastList();
        return ApiResponse<List<FactForecastListDto>>.Success(result, 200);
    }

    public async Task<ApiResponse<List<QuarterListDto>>> GetQuarterListAsync()
    {
        var result = await _barrelFlowRepository.GetQuarterList();
        return ApiResponse<List<QuarterListDto>>.Success(result, 200);
    }

    public async Task<ApiResponse<decimal>> GetBarrelFlowRegisterOpeningPeriodAsync(int businessUnitId,
        int period)
    {
        var result = await _barrelFlowRepository.GetBarrelFlowRegisterOpeningPeriod(businessUnitId, period);
        return ApiResponse<decimal>.Success(result);
    }
}