using SolaERP.Infrastructure.Entities.Vendors;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IVendorRepository : ICrudOperations<Vendors>
    {
        public Task<List<Vendors>> GetVendorDrafts(int userId, int businessUnitId);
        public Task<List<Vendors>> GetVendorWFA(int userId, int businessUnitId);

    }
}
