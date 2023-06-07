using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface ISupplierEvaluationService
    {
        Task<ApiResponse<List<DueDiligenceDesignDto>>> GetDueDiligenceAsync(Enums.Language language);
        Task<ApiResponse<PrequalificationDto>> GetPrequalificationAsync(string userIdentity);
        Task<ApiResponse<VM_GET_SupplierEvaluation>> GetAllAsync(SupplierEvaluationGETModel model);
        Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync(string userIdentity);
        Task<ApiResponse<VM_GET_VendorBankDetails>> GetBankDetailsAsync(string userIdentity);
        Task<ApiResponse<List<NonDisclosureAgreement>>> GetNDAAsync(string userIdentity);
        Task<ApiResponse<List<CodeOfBuConduct>>> GetCOBCAsync(string userIdentity);
        Task<ApiResponse<BankCodesDto>> GetBankCodesAsync(int vendorId);
    }
}
