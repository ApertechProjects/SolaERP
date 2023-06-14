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
        Task<List<VendorDueDiligence>> GetVendorDuesAsync(int vendorId);
        Task<List<VendorPrequalification>> GetVendorPrequalificationAsync(int vendorId);
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

        Task<bool> AddPreGriAsync(Entities.SupplierEvaluation.PrequalificationGridData grid);
        Task<bool> UpdatePreGriAsync(Entities.SupplierEvaluation.PrequalificationGridData grid);
        Task<bool> DeletePreGriAsync(int preGridId);
        Task<bool> PrequalificationCategoryAddAsync(PrequalificationCategoryData data);
        Task<bool> PrequalificationCategoryDeleteAsync(int vendorId);
        Task<bool> VendorBusinessCategoryAddAsync(VendorBusinessCategoryData data);
        Task<bool> VendorBusinessCategoryDeleteAsync(int vendorId);
        Task<bool> VendorRepresentedCompanyAddAsync(VendorRepresentedCompany data);
        Task<bool> VendorRepresentedCompanyDeleteAsync(int vendorId);
        Task<bool> VendorRepresentedProductAddAsync(RepresentedProductData data);
        Task<bool> VendorRepresentedProductDeleteAsync(int vendorId);
        Task<bool> AddPreGridAsync(Entities.SupplierEvaluation.PrequalificationGridData grid);
        Task<bool> UpdatePreGridAsync(Entities.SupplierEvaluation.PrequalificationGridData grid);
        Task<bool> DeletePreGridAsync(int preGridId);

    }
}
