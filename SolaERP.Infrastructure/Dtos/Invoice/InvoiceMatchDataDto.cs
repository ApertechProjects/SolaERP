using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Invoice
{
    public class InvoiceMatchMainDataDto
    {
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public string Currency { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public int WithHoldingTaxId { get; set; }
        public string WithHoldingTaxCode { get; set; }
        public decimal WithHoldingTax { get; set; }
        public decimal OrderTotal { get; set; }
    }

    public class InvoiceMatchDetailDataDto
    {
        public int InvoiceMatchingDetailId { get; set; }
        public int InvoiceMatchingMainId { get; set; }
        public string LineNo { get; set; }
        public string OrderNo { get; set; }
        public int OrderLine { get; set; }
        public string GRNNo { get; set; }
        public int GRNLine { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string MWP { get; set; }
        public decimal GRNQTY { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public decimal AdvanceTotal { get; set; }
        public DateTime GRNDate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal ReportingAmount { get; set; }
        public string Description { get; set; }
        public int AnalysisCode1Id { get; set; }
        public int AnalysisCode2Id { get; set; }
        public int AnalysisCode3Id { get; set; }
        public int AnalysisCode4Id { get; set; }
        public int AnalysisCode5Id { get; set; }
        public int AnalysisCode6Id { get; set; }
        public int AnalysisCode7Id { get; set; }
        public int AnalysisCode8Id { get; set; }
        public int AnalysisCode9Id { get; set; }
        public int AnalysisCode10Id { get; set; }
        public string AccountCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }

    public class InvoiceMatchAdvanceDto
    {
        public int InvoiceMatchingAdvanceId { get; set; }
        public int InvoiceMatchingMainId { get; set; }
        public int InvoiceRegisterId { get; set; }
        public decimal AllocatedAmount { get; set; }
    }

    public class InvoiceMatchGRNDto
    {
        public int InvoiceMatchingGRNId { get; set; }
        public int InvoiceMatchingMainId { get; set; }
        public string GRNReference { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}
