using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IVendorRepository : ICrudOperations<Vendors>
    {
        public Task<List<Vendors>> GetVendorDrafts(int userId, int businessUnitId);
        public Task<List<Vendors>> GetVendorWFA(int userId, int businessUnitId);
        public Task<VendorInfo> GetVendorByTaxIdAsync(string taxId);
    }
}
