using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.Vendors;
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
        Task<ApiResponse<List<VendorInfoDto>>> Vendors(string userName);
        Task<ApiResponse<List<VendorAll>>> GetDraftAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorAllDto>>> GetAllAsync(string userIdentity, VendorAllCommandRequest request);
        Task<ApiResponse<List<VendorWFADto>>> GetHeldAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorWFADto>>> GetRejectedAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<bool>> ChangeStatusAsync(VendorStatusModel taxModel, string userIdentity);
        Task<ApiResponse<VM_VendorCard>> GetAsync(int vendorId);
        Task<ApiResponse<bool>> ApproveAsync(string userIdentity, VendorApproveModel model);
        Task<ApiResponse<bool>> SendToApproveAsync(VendorSendToApproveRequest request);
        Task<ApiResponse<bool>> SaveAsync(string userIdentity, VendorCardDto vendor);
        Task<ApiResponse<List<VendorAllDto>>> GetApprovedAsync(string userIdentity, string text);
        Task<ApiResponse<bool>> DeleteAsync(string userIdentity, VendorDeleteModel model);
        Task<ApiResponse<bool>> HasVendorName(string vendorName, string userIdentity);
    }
}
