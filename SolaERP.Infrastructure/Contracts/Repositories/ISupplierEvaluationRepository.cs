using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ISupplierEvaluationRepository
    {
        Task<List<Country>> GetCountriesAsync();
        Task<List<BusinessCategory>> GetBusinessCategoriesAsync();
        Task<List<PrequalificationCategory>> GetPrequalificationCategoriesAsync();
        Task<List<ProductService>> GetProductServicesAsync();
        Task<List<PaymentTerms>> GetPaymentTermsAsync();
        Task<List<Currency>> GetCurrenciesAsync();
        Task<List<DueDiligenceDesign>> GetDueDiligencesDesignAsync(Enums.Language language);
        Task<List<DueDiligenceGrid>> GetDueDiligenceGridsAsync(int deuDesignId);
        Task<List<VendorBankDetails>> GetVondorBankDetailsAsync(int vendorid);
        Task<bool> AddDueDesign(VendorDueDiligenceModel vendorDueDiligence);
        Task<bool> AddDueDesignGrid(DueDiligenceGridModel gridModel);
        Task<bool> UpdateDueDesignGrid(DueDiligenceGridUpdateModel gridModel);
        Task<bool> DeleteDueDesignGrid(int id);
        Task<bool> AddPrequalificationAsync(VendorPreInputModel model);
        Task<bool> UpdatePrequalificationAsync(int id, VendorPreInputModel model);
        Task<bool> DeletePrequalificationAsync(int id);
        Task<bool> AddDueDiligenceAsync(VendorDueDiligenceModel model);
        Task<bool> UpdateDueDiligenceAsync(int id, VendorDueDiligenceModel model);
        Task<bool> DeleteDueDiligenceAsync(int id);
        Task<bool> AddVendorAsync(VendorModel model);
        Task<bool> UpdateVendorAsync(int id, VendorModel model);
        Task<bool> DeleteVendorAsync(int id);
    }
}
