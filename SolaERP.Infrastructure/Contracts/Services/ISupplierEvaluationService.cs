using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface ISupplierEvaluationService
    {
        Task<ApiResponse<List<DueDiligenceDesignDto>>> GetDueDiligenceAsync(string userIdentity, string acceptLanguage, int? vendorId = null);
        Task<ApiResponse<VM_GET_SupplierEvaluation>> GetAllAsync(SupplierEvaluationGETModel model);
        Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync(string userIdentity, int? vendorId = null);
        Task<ApiResponse<VM_GET_VendorBankDetails>> GetBankDetailsAsync(string userIdentity, int? vendorId = null);
        Task<ApiResponse<List<NonDisclosureAgreement>>> GetNDAAsync(string userIdentity, int? vendorId = null);
        Task<ApiResponse<List<CodeOfBuConduct>>> GetCOBCAsync(string userIdentity);
        Task<ApiResponse<List<PrequalificationWithCategoryDto>>> GetPrequalificationAsync(string userIdentity, List<int> categoryIds, string acceptlang, int? vendorId = null);
        Task<ApiResponse<int>> AddAsync(string userIdentity, string Token, SupplierRegisterCommand command);
        Task<ApiResponse<int>> SubmitAsync(string userIdentity, string Token, SupplierRegisterCommand command);
        Task<ApiResponse<bool>> UpdateVendor(string name, string taxId);
    }
}
