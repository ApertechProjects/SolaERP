using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Models;
using SolaERP.Application.ViewModels;

namespace SolaERP.Application.Contracts.Services
{
    public interface IVendorService
    {
        Task<ApiResponse<VM_GetVendorFilters>> GetFiltersAsync();
        Task<int> GetByTaxIdAsync(string taxId);
        Task<VendorInfo> GetByTaxAsync(string taxId);
        Task<ApiResponse<List<VendorWFA>>> GetWFAAsync(string userIdentity, VendorFilter filter);
    }
}
