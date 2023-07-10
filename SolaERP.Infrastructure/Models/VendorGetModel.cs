using SolaERP.Application.Dtos.SupplierEvaluation;
using SolaERP.Application.Dtos.Vendors;
using SolaERP.Application.Entities.Currency;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Currency = SolaERP.Application.Entities.Currency.Currency;

namespace SolaERP.Application.Models
{
    public class VendorGetModel
    {
        public VendorCardDto Header { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<PaymentTerms> PaymentTerms { get; set; }
        public List<DeliveryTerms> DeliveryTerms { get; set; }
        public List<VendorBankDetailDto> VendorBankDetails { get; set; }
        public List<VendorUser> VendorUsers { get; set; }
        public List<VendorBuCategory> VendorBuCategories { get; set; }
        public List<Score> Score { get; set; }
        public List<Shipment> Shipments { get; set; }
        public List<WithHoldingTaxData> WithHoldingTaxDatas { get; set; }
        public List<TaxData> TaxDatas { get; set; }
    }
}
