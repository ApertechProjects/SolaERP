using System.Data;
using SolaERP.Application.Dtos.BarrelFlow;

namespace SolaERP.Application.Contracts.Repositories;

public interface IBarrelFlowRepository
{
    Task<bool> SaveBarrelFlowsRegisterIUD(DataTable dataTable);
    Task<List<BarrelFlowRegisterDto>> GetBarrelFlowRegister(int businessUnitId, DateTime dateFrom, DateTime dateTo);
}