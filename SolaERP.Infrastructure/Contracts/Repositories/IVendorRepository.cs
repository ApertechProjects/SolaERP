
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IVendorRepository
    {
        Task<bool> AddBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<bool> UpdateBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<bool> DeleteBankDetailsAsync(int userId, int id);
        Task<int> AddVendorAsync(int userId, Vendor vendor);
        Task<int> UpdateVendorAsync(int userId, Vendor vendor);
        Task<int> DeleteVendorAsync(int userId, int id);
        Task<int> GetVendorByTaxIdAsync(string taxId);
        Task<VendorInfo> GetVendorByTaxIdAsync(string taxId);
        Task<bool> VendorChangeStatus(int vendorId, int status, int userId);
    }
}
