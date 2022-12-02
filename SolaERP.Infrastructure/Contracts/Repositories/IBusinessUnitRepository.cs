using SolaERP.Infrastructure.Entities.BusinessUnits;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IBusinessUnitRepository : ICrudOperations<BusinessUnits>
    {
        Task<List<BusinessUnits>> GetBusinessUnitListByUserId(int userId);
    }
}
