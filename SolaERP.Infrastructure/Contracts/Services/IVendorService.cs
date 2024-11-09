using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Payment;
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
        Task<ApiResponse<List<VendorBaseInfoDto>>> Vendors(string userName);
        Task<ApiResponse<List<VendorDraft>>> GetDraftAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorWFADto>>> GetWFAAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorAllDto>>> GetAllAsync(string userIdentity, VendorAllCommandRequest request);
        Task<ApiResponse<List<VendorWFADto>>> GetHeldAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<List<VendorWFADto>>> GetRejectedAsync(string userIdentity, VendorFilter filter);
        Task<ApiResponse<bool>> ChangeStatusAsync(VendorStatusModel taxModel, string userIdentity, HttpResponse response);
        Task<ApiResponse<VM_VendorCard>> GetAsync(int vendorId);
        Task<VendorLoadDto> GetVendorPreviousHeader(int vendorId);
        Task<ApiResponse<bool>> ApproveAsync(string userIdentity, VendorApproveModel model, HttpResponse response);
        Task<ApiResponse<bool>> SendToApproveAsync(VendorSendToApproveRequest request);
        Task<ApiResponse<bool>> SaveAsync(string userIdentity, VendorCardDto vendor);
        Task<ApiResponse<bool>> SaveAsync2(string userIdentity, VendorCardDto2 vendor);
        Task<ApiResponse<List<VendorApprovedDto>>> GetApprovedAsync(string userIdentity, string text);
        Task<ApiResponse<bool>> DeleteAsync(string userIdentity, VendorDeleteModel model);
        Task<ApiResponse<bool>> HasVendorName(string vendorName, string userIdentity);
        Task<ApiResponse<List<VendorRFQListResponseDto>>> GetVendorRFQList(string vendorCode, string userIdentity);
        Task<ApiResponse<bool>> RFQVendorResponseChangeStatus(int rfqMainId, int status, string vendorCode);
        Task<ApiResponse<bool>> TransferToIntegration(CreateVendorRequest request);
        Task<ApiResponse<List<string>>> CompareVendor(int oldVersionId, int newVersionId);
    }
}