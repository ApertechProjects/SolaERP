using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Invoice
{
    public class InvoiceRegisterOrderDetailDto
	{
		public Int64 LineNo { get; set; }
		public string? Description { get; set; }
		public string ItemCode { get; set; }
		public decimal OrderAmount { get; set; }
		public int? Amount { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal GrossAmount { get; set; }
		public string? AccountCode { get; set; }
		public decimal Quantity { get; set; }
		public string RUOM { get; set; }
		public int BaseAmount { get; set; }
		public int ReportingAmount { get; set; }
		public int BaseGrossAmount { get; set; }
		public int WithHoldingTaxAmount { get; set; }
		public int BaseWithHoldingTaxAmount { get; set; }
		public int ReportingWithHoldingTaxAmount { get; set; }
		public int ReportingGrossAmount { get; set; }
		public int CatId { get; set; }
		public int? AnalysisCode1Id { get; set; }
		public int? AnalysisCode2Id { get; set; }
		public int? AnalysisCode3Id { get; set; }
		public int? AnalysisCode4Id { get; set; }
		public int? AnalysisCode5Id { get; set; }
		public int? AnalysisCode6Id { get; set; }
		public int? AnalysisCode7Id { get; set; }
		public int? AnalysisCode8Id { get; set; }
		public int? AnalysisCode9Id { get; set; }
		public int? AnalysisCode10Id { get; set; }
	}
}
