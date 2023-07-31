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
        Task<ApiResponse<bool>> AddAsync(string userIdentity, SupplierRegisterCommand command);
        Task<ApiResponse<bool>> SubmitAsync(string userIdentity, SupplierRegisterCommand command);

    }
}
