using SolaERP.Infrastructure.Entities.BusinessUnits;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IBusinessUnitRepository : ICrudOperations<BusinessUnits>
    {
        Task<List<BaseBusinessUnit>> GetBusinessUnitListByUserId(int userId);
        Task<List<BusinessUnitForGroup>> GetBusinessUnitForGroups(int groupId);
    }
}
