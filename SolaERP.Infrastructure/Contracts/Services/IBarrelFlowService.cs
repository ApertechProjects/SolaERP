using SolaERP.Application.Dtos.BarrelFlow;
using SolaERP.Application.Dtos.BidComparison;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Application.Contracts.Services;

public interface IBarrelFlowService
{
    Task<ApiResponse<bool>> SaveBarrelFlowsAsync(List<BarrelFlowUIDDto> data);

    Task<ApiResponse<List<BarrelFlowRegisterDto>>> BarrelFlowRegisterAsync(int businessUnitId,
        DateTime dateFrom, DateTime dateTo);

}