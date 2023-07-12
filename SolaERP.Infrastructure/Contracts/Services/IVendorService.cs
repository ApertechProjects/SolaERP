using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Dtos.Venndors;
using SolaERP.Application.Entities.Status;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.ViewModels;

namespace SolaERP.Application.Contracts.Services
{
    public interface IVendorService
    {
        Task<ApiResponse<VM_GetVendorFilters>> GetFiltersAsync();
        Task<int> GetByTaxIdAsync(string taxId);
        Task<VendorInfo> GetByTaxAsync(string taxId);
        Task<ApiResponse<List<VendorAll>>> GetDraftAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorAllDto>>> GetAllAsync(string userIdentity, VendorAllCommandRequest request);
        Task<ApiResponse<List<VendorWFADto>>> GetHeldAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<bool>> ChangeStatusAsync(TaxModel taxModel, string userIdentity);
        Task<ApiResponse<VendorGetModel>> GetVendorCard(int vendorId);
        Task<ApiResponse<bool>> ApproveAsync(string userIdentity, VendorApproveModel model);
        Task<ApiResponse<bool>> SendToApproveAsync(int vendorId);
    }
}
