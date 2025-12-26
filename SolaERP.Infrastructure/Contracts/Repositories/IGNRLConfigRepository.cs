using SolaERP.Application.Entities;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IGNRLConfigRepository
    {
        Task<List<GNRLConfig>> GetGNRLConfigsByBusinessUnitId(int businessUnitId);
    }
}