using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Contracts.Services
{
    public interface IVendorService : ICrudService<Vendors>
    {
        Task<int> GetVendorByTaxIdAsync(string taxId);
        Task<VendorInfo> GetVendorByTaxAsync(string taxId);
        Task<ApiResponse<List<VendorWFA>>> WaitingForApprovals(string userName);
        Task<ApiResponse<List<VendorAll>>> All(string userName);
    }
}
