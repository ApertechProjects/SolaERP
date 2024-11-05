using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
	public class InvoiceRegisterGetDetails : BaseEntity
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
	}
}
