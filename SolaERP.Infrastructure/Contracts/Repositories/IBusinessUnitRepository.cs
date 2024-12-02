using SolaERP.Application.Entities.BusinessUnits;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IBusinessUnitRepository : ICrudOperations<BusinessUnits>
    {
        Task<List<BaseBusinessUnit>> GetBusinessUnitListByUserId(int userId);
        Task<List<BaseBusinessUnit>> GetBusinessUnitListByCurrentUser(int userId);
        Task<string> GetBusinessUnitCode(int businessUnitId);
        Task<string> GetBusinessUnitName(int businessUnitId);
	}
}