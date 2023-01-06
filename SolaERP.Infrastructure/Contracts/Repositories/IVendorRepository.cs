using SolaERP.Infrastructure.Entities.Vendors;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IVendorRepository : ICrudOperations<Vendor>
    {
        public Task<List<Vendor>> GetVendorDrafts(int userId, int businessUnitId);
        public Task<List<Vendor>>GetVendorWFA(int userId, int businessUnitId);  

    }
}
