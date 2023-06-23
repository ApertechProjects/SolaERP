
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IVendorRepository
    {
        Task<int> AddBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<int> UpdateBankDetailsAsync(int userId, VendorBankDetail bankDetail);
        Task<int> DeleteBankDetailsAsync(int userId, int id);
        Task<int> AddVendorAsync(int userId, Vendor vendor);
        Task<int> UpdateVendorAsync(int userId, Vendor vendor);
        Task<int> DeleteVendorAsync(int userId, int id);
        Task<VendorInfo> GetVendorByTaxAsync(string taxId);
        Task<bool> VendorChangeStatus(int vendorId, int status, int userId);
        Task<List<VendorWFA>> WaitingForApprovals(int userId);
        Task<List<VendorAll>> All(int userId);
        Task<List<VendorInfo>> Get(int businessUnitId, int userId);
    }
}
