
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;

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
        Task<List<VendorWFA>> GetWFAAsync(int userId, VendorFilter filter);
        Task<List<VendorAll>> GetAll(int userId, VendorFilter filter, int status, int approval);
        Task<List<VendorWFA>> GetHeldAsync(int userId, VendorFilter filter);
        Task<List<VendorAll>> GetDraftAsync(int userId, VendorFilter filter);
        Task<List<VendorInfo>> Get(int businessUnitId, int userId);
    }
}
