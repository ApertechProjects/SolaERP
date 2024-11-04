using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
	public class InvoiceRegisterDetailsSaveModel
	{
		public int InvoiceRegisterMainId { get; set; }
        public List<InvoiceRegisterDetails> Details { get; set; }
    }

	public class InvoiceRegisterDetails
	{
		public int InvoiceregisterDetailId { get; set; }
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
		public int CatId { get; set; }
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


	}
}
