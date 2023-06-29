
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IVendorRepository
    {
        Task<int> AddBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<int> UpdateBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<int> DeleteBankDetailsAsync(int userId, int id);
        Task<int> AddAsync(int userId, Vendor vendor);
        Task<int> UpdateAsync(int userId, Vendor vendor);
        Task<int> DeleteAsync(int userId, int id);
        Task<VendorInfo> GetByTaxAsync(string taxId);
        Task<bool> ChangeStatusAsync(int vendorId, int status, int userId);
        Task<List<VendorWFA>> GetWFAAsync(int userId);
        Task<List<VendorAll>> GetAll(int userId);
        Task<List<VendorInfo>> Get(int businessUnitId, int userId);
    }
}
