using SolaERP.Application.Dtos.BusinessCategory;
using SolaERP.Application.Dtos.Country;
using SolaERP.Application.Dtos.Currency;
using SolaERP.Application.Dtos.DeliveryTerm;
using SolaERP.Application.Dtos.PaymentTerm;
using SolaERP.Application.Dtos.Score;
using SolaERP.Application.Dtos.Shipment;
using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.TaxDto;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Dtos.WithHoldingTax;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Models
{
    public class VM_VendorCard
    {
        public VendorLoadDto Header { get; set; }
        public VendorLoadDto HeaderPrevious { get; set; }
        public List<CurrencyDto> Currencies { get; set; }
        public List<PaymentTermDto> PaymentTerms { get; set; }
        public List<DeliveryTermDto> DeliveryTerms { get; set; }
        public List<VendorBankDetailViewDto> VendorBankDetails { get; set; }
        public List<VendorBankDetailViewDto> VendorBankDetailsPrevious { get; set; }
        public List<VendorUserDto> VendorUsers { get; set; }
        public List<VendorUserDto> VendorUsersPrevious { get; set; }
        public List<BusinessCategoryDto> ItemCategories { get; set; }
        public List<BusinessCategoryDto> ItemCategoriesPrevious { get; set; }
        public List<ScoreDto> Score { get; set; }
        public List<ScoreDto> ScorePrevious { get; set; }
        public List<ShipmentDto> Shipments { get; set; }
        public List<WithHoldingTaxDto> WithHoldingTaxDatas { get; set; }
        public List<TaxDto> TaxDatas { get; set; }
        public List<CountryDto> Countries { get; set; }
        public List<VendorBuCategory> ActiveItemCategories { get; set; }
    }
}
