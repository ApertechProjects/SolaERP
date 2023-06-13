using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ISupplierEvaluationRepository
    {
        Task<List<VendorNDA>> GetNDAAsync(int vendorId);
        Task<List<PrequalificationDesign>> GetPrequalificationDesignsAsync(int categoryId, Language language);
        Task<List<VendorCOBC>> GetCOBCAsync(int vendorId);
        Task<List<Country>> GetCountriesAsync();
        Task<List<BusinessCategory>> GetBusinessCategoriesAsync();
        Task<List<PrequalificationCategory>> GetPrequalificationCategoriesAsync();
        Task<List<ProductService>> GetProductServicesAsync();
        Task<List<PaymentTerms>> GetPaymentTermsAsync();
        Task<List<Currency>> GetCurrenciesAsync();
        Task<List<DueDiligenceDesign>> GetDueDiligencesDesignAsync(Enums.Language language);
        Task<List<VendorProductService>> GetVendorProductServices(int vendorId);
        Task<List<DueDiligenceGrid>> GetDueDiligenceGridAsync(int dueDesignId);
        Task<List<Entities.SupplierEvaluation.PrequalificationGridData>> GetPrequalificationGridAsync(int preDesignId);
        Task<List<VendorBankDetail>> GetVondorBankDetailsAsync(int vendorid);
        Task<CompanyInfo> GetCompanyInfoAsync(int vendorId);
        Task<List<Entities.SupplierEvaluation.VendorDueDiligence>> GetVendorDuesAsync(int vendorId);
        Task<List<Entities.SupplierEvaluation.VendorPrequalification>> GetVendorPrequalificationAsync(int vendorId);
        Task<List<VendorBuCategory>> GetVendorBuCategoriesAsync(int vendorId);
        Task<Prequalification> GetPrequalificationAsync(int vendorid);
        Task<List<VendorPrequalificationValues>> GetPrequalificationValuesAsync(int vendorId);




        Task<bool> AddPrequalification(VendorPrequalificationValues value);
        Task<bool> UpdatePrequalification(VendorPrequalificationValues value);
        Task<bool> DeletePrequalification(int vendorPreId);


        Task<bool> AddNDAAsync(VendorNDA ndas);
        Task<bool> DeleteNDAAsync(int ndaId);
        Task<bool> AddCOBCAsync(VendorCOBC cobc);
        Task<bool> DeleteCOBCAsync(int id);
        Task<bool> AddDueDesignGrid(DueDiligenceGridModel gridModel);
        Task<bool> UpdateDueDesignGrid(DueDiligenceGridModel gridModel);
        Task<bool> DeleteDueDesignGrid(int id);
        Task<bool> AddDueAsync(VendorDueDiligenceModel model);
        Task<bool> UpdateDueAsync(VendorDueDiligenceModel model);
        Task<bool> DeleteDueAsync(int dueId);
    }
}
