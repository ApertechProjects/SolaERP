using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ISupplierEvaluationRepository
    {
        Task<List<VendorNDA>> GetNDAAsync(int vendorId);
        Task<List<VendorCOBC>> GetCOBCAsync(int vendorId);
        Task<List<Country>> GetCountriesAsync();
        Task<List<BusinessCategory>> GetBusinessCategoriesAsync();
        Task<List<PrequalificationCategory>> GetPrequalificationCategoriesAsync();
        Task<List<ProductService>> GetProductServicesAsync();
        Task<List<PaymentTerms>> GetPaymentTermsAsync();
        Task<List<Currency>> GetCurrenciesAsync();
        Task<List<DueDiligenceDesign>> GetDueDiligencesDesignAsync(Enums.Language language);
        Task<List<DueDiligenceGrid>> GetDueDiligenceGridAsync(int deuDesignId);
        Task<List<VendorBankDetail>> GetVondorBankDetailsAsync(int vendorid);
        Task<CompanyInfo> GetCompanyInfoAsync(int vendorId);
        Task<List<VendorPrequalification>> GetVendorPrequalificationAsync(int vendorId);
        Task<List<VendorBuCategory>> GetVendorBuCategoriesAsync(int vendorId);
        Task<Prequalification> GetPrequalificationAsync(int vendorid);
        Task<bool> AddNDAAsync(VendorNDA ndas);
        Task<bool> DeleteNDAAsync(int ndaId);
        Task<bool> AddCOBCAsync(VendorCOBC cobc);
        Task<bool> DeleteCOBCAsync(int id);
        Task<bool> AddDueDesignGrid(DueDiligenceGridModel gridModel);
        Task<bool> UpdateDueDesignGrid(DueDiligenceGridModel gridModel);
        Task<bool> DeleteDueDesignGrid(int id);
        Task<bool> AddDueDesignAsync(VendorDueDiligenceModel model);
        Task<bool> UpdateDueDesignAsync(VendorDueDiligenceModel model);
        Task<bool> DeleteDueDesignAsync(int dueId);
    }
}
