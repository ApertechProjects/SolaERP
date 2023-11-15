using SolaERP.Application.Entities.Location;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ILocationRepository : ICrudOperations<Location>
    {
        Task<List<Location>> GetAllByBusinessUnitId(int businessUnitId);
    }
}
