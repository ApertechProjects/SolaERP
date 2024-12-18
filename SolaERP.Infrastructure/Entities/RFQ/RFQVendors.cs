using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.RFQ
{
	public class RFQVendors : BaseEntity
	{
		public int RowNumber { get; set; }
		public string VendorCode { get; set; }
		public string VendorName { get; set; }
	}
}
