using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Invoice
{
	public class InvoiceRegisterGetDetailsDto
	{
		public int InvoiceRegisterDetailId { get; set; }
		public int LineNo { get; set; }
		public string LineDescription { get; set; }
		public decimal Amount { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal GrossAmount { get; set; }
		public string AccountCode { get; set; }
		public decimal QTY { get; set; }
		public string UOM { get; set; }
		public decimal BaseAmount { get; set; }
		public decimal ReportingAmount { get; set; }
		public decimal BaseTaxAmount { get; set; }
		public decimal ReportingTaxAmount { get; set; }
		public decimal BaseGrossAmount { get; set; }
		public decimal ReportingGrossAmount { get; set; }
        public decimal WithHoldingTaxAmount { get; set; }
		public decimal BaseWithHoldingTaxAmount { get; set; }
		public decimal ReportingWithHoldingTaxAmount { get; set; }
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
		public string ItemCode { get; set; }
		public decimal? OrderAmount { get; set; }
	}
}
