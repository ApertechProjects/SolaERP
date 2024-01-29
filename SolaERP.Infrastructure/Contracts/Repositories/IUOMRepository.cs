using SolaERP.Application.Entities.UOM;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IUOMRepository : ICrudOperations<UOM>
    {
        Task<List<UOM>> GetUOMListBusinessUnitCode(int businessUnitId);
    }
}
