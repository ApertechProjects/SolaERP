using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.VendorDueDiligence;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ISupplierEvaluationRepository
    {
        Task<List<VendorNDA>> GetNDAAsync(int vendorId);
        Task<Entities.Vendors.VendorRepresentedCompany> GetRepresentedCompanyAsync(int vendorId);
        Task<VendorRepresentedProduct> GetRepresentedProductAsync(int vendorId);
        Task<List<PrequalificationDesign>> GetPrequalificationDesignsAsync(int categoryId, Language language);
        Task<List<VendorCOBC>> GetCOBCAsync(int vendorId);
        Task<List<Country>> GetCountriesAsync();
        Task<List<BusinessCategory>> GetBusinessCategoriesAsync();
        Task<List<PrequalificationCategory>> GetPrequalificationCategoriesAsync();
        Task<List<ProductService>> GetProductServicesAsync();
        Task<List<PaymentTerms>> GetPaymentTermsAsync();
        Task<List<DeliveryTerms>> GetDeliveryTermsAsync();
        Task<List<Currency>> GetCurrenciesAsync();
        Task<List<DueDiligenceDesign>> GetDueDiligencesDesignAsync(Enums.Language language);
        Task<List<VendorProductService>> GetVendorProductServices(int vendorId);
        Task<List<DueDiligenceGrid>> GetDueDiligenceGridAsync(int dueDesignId, int vendorId);
        Task<List<Entities.SupplierEvaluation.PrequalificationGridData>> GetPrequalificationGridAsync(int vendorId);
        Task<List<VendorBankDetail>> GetVendorBankDetailsAsync(int vendorid);
        Task<CompanyInfo> GetCompanyInfoAsync(int vendorId);
        Task<List<DueDiligenceValue>> GetVendorDuesAsync(int vendorId);
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

        Task<bool> AddPreGridAsync(Entities.SupplierEvaluation.PrequalificationGridData grid);
        Task<bool> UpdatePreGridAsync(Entities.SupplierEvaluation.PrequalificationGridData grid);
        Task<bool> DeletePreGridAsync(int preGridId);
        Task<bool> AddPrequalificationCategoryAsync(PrequalificationCategoryData data);
        Task<bool> DeletePrequalificationCategoryAsync(int vendorId);
        Task<bool> AddVendorBusinessCategoryAsync(VendorBusinessCategoryData data);
        Task<bool> DeleteVendorBusinessCategoryAsync(int vendorId);
        Task<bool> AddRepresentedCompany(Models.VendorRepresentedCompany data);
        Task<bool> DeleteRepresentedCompanyAsync(int vendorId);
        Task<bool> AddRepresentedProductAsync(RepresentedProductData data);
        Task<bool> DeleteRepresentedProductAsync(int vendorId);

        Task<bool> AddProductServiceAsync(ProductServiceData productService);
        Task<bool> DeleteProductServiceAsync(int id);

        Task<bool> AddPequalificationCategoryAsync(PrequalificationCategory category);
        Task<bool> DeletePequalificationCategoryAsync(int id);

        Task<List<VendorUser>> GetVendorUsers(int vendorId);
        Task<List<Score>> Scores(int vendorId);
        Task<List<Shipment>> Shipments();
        Task<List<WithHoldingTaxData>> WithHoldingTaxDatas();
        Task<List<TaxData>> TaxDatas();
    }
}
