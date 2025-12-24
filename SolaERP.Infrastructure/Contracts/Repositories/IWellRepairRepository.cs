using SolaERP.Application.Dtos.WellRepair;

namespace SolaERP.Application.Contracts.Repositories;

public interface IWellRepairRepository
{
    Task<List<WellRepairListDto>> GetWellRepairList();
}