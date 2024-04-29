using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;

namespace SolaERP.Application.Models
{
    public class VM_VendorCard
    {
        public VendorLoadDto Header { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<DeliveryTerms> DeliveryTerms { get; set; }
        public List<VendorBankDetailViewDto> VendorBankDetails { get; set; }
        public List<VendorUser> VendorUsers { get; set; }
        public List<BusinessCategory> ItemCategories { get; set; }
        public List<Score> Score { get; set; }
        public List<Shipment> Shipments { get; set; }
        public List<WithHoldingTaxData> WithHoldingTaxDatas { get; set; }
        public List<TaxData> TaxDatas { get; set; }
        public List<Country> Countries { get; set; }
    }
}
