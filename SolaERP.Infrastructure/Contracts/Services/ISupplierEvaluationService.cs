
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface ISupplierEvaluationService
    {
        Task<List<DueDiligenceDesignDto>> GetDueDiligenceAsync(Enums.Language language);
        Task<ApiResponse<VM_GET_SupplierEvaluation>> GetAllAsync(SupplierEvaluationGETModel model);
        Task<ApiResponse<VM_GET_InitalRegistration>> GetInitRegistrationAsync();
        Task<ApiResponse<BankCodesDto>> GetBankCodesAsync(int vendorId);

    }
}
