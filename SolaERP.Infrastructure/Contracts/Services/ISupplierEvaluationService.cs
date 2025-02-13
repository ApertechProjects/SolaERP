using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface ISupplierEvaluationService
    {
        Task<ApiResponse<List<DueDiligenceDesignDto>>> GetDueDiligenceAsync(string userIdentity, string acceptLanguage,
            int? vendorId = null);

        Task<ApiResponse<VM_GET_SupplierEvaluation>> GetAllAsync(SupplierEvaluationGETModel model);

        Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync(string userIdentity, int? vendorId = null);

        Task<ApiResponse<VM_GET_VendorBankDetails>> GetBankDetailsAsync(string userIdentity, int? vendorId = null);
        Task<ApiResponse<List<NonDisclosureAgreement>>> GetNDAAsync(string userIdentity, int? vendorId = null);
        Task<ApiResponse<List<CodeOfBuConduct>>> GetCOBCAsync(string userIdentity, int? vendorId = null);

        Task<ApiResponse<List<PrequalificationWithCategoryDto>>> GetPrequalificationAsync(string userIdentity,
            List<int> categoryIds, string acceptlang, int? vendorId = null);

        Task<ApiResponse<List<PrequalificationWithCategoryDto2>>> GetPrequalificationAsync2(string userIdentity,
          int categoryId, string acceptlang, int? vendorId = null);

        #region
        //Task<ApiResponse<int>> AddAsync(string userIdentity, string token, SupplierRegisterCommand command,
        //    bool isSubmitted = false, bool isRevise = false);

        //Task<ApiResponse<int>> SubmitAsync(string userIdentity, string token, SupplierRegisterCommand command,
        //    bool isRevise);
        #endregion

        Task<ApiResponse<EvaluationResultModel>> AddAsync2(string userIdentity, SupplierRegisterCommand2 command,
         bool isSubmitted = false);

        Task<ApiResponse<EvaluationResultModel>> SubmitAsync2(string userIdentity, SupplierRegisterCommand2 command);

        Task<ApiResponse<bool>> UpdateVendor(string name, string taxId);
        Task UpdateAsync(string userIdentity, SupplierRegisterCommand2 command);

        Task<ApiResponse<EvaluationResultModel>> UpdateVendorRevise(string useridentity,
            SupplierRegisterCommand2 command, bool isSubmitted = false);

    }
}