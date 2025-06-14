using SolaERP.Application.Dtos.BusinessUnit;
using SolaERP.Application.Dtos.TaxDto;
using SolaERP.Application.Dtos.WithHoldingTax;
using SolaERP.Application.Entities.BusinessUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
    public class InvoiceRegisterLoad : BaseEntity
    {
        private decimal? _grossAmount;
        private decimal? _taxAmount;
        public int InvoiceRegisterId { get; set; }
        public int BusinessUnitId { get; set; }
        public int InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? InvoiceNo { get; set; }
        public string? SystemInvoiceNo { get; set; }
        public int? OrderType { get; set; }
        public string? ReferenceDocNo { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string? LineDescription { get; set; }
        public string? VendorCode { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AgingDays { get; set; }
        public int? ProblematicInvoiceReasonId { get; set; }
        public int? Status { get; set; }
        public int? ApproveStatus { get; set; }
        public string? ReasonAdditionalDescription { get; set; }
        public string? Comment { get; set; }
        public int? OrderMainId { get; set; }
        public string? AccountCode { get; set; }
        public int? WithHoldingTaxId { get; set; }
        public int? TaxId { get; set; }
        public int InvoiceTransactionTypeId { get; set; }
        public bool FullPrepaid { get; set; }
        public int? LinkedInvoiceRegisterId { get; set; }
        
        public decimal? TaxAmount
        {
            get => _taxAmount ?? 0;
            set => _taxAmount = value ?? 0;
        }
        public decimal? GrossAmount
        {
            get => _grossAmount ?? 0;
            set => _grossAmount = value ?? 0;
        }
        public decimal WithHoldingTaxAmount { get; set; }
        public string? OrderReference { get; set; }
        public int? InvoicePeriod { get; set; }
        public string? VendorAccount { get; set; }
        public bool UseOrderForInvoice { get; set; }
        List<InvoiceRegisterGetDetails> InvoiceRegisterDetails { get; set; }
        public List<Vendors.WithHoldingTaxData> WithHoldingTaxDatas { get; set; }
        public List<Vendors.TaxData> TaxDatas { get; set; }
        public List<BaseBusinessUnit> BusinessUnits { get; set; }

    }


}
