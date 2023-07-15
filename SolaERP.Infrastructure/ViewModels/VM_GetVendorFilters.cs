using SolaERP.Application.Entities.SupplierEvaluation;

namespace SolaERP.Application.ViewModels
{
    public class VM_GetVendorFilters
    {
        public List<PrequalificationCategory> PrequalificationCategories { get; set; }
        public List<BusinessCategory> BusinessCategories { get; set; }
        public List<ProductService> ProductServices { get; set; }

    }
}
