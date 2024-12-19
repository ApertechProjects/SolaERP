using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job.RFQClose
{
	public class RFQUserData
	{
        public int RFQVendorResponseId { get; set; }
        public int RFQMainId { get; set; }
        public string Language { get; set; }
		public string VendorCode { get; set; }
		public string VendorName { get; set; }
		public string Email { get; set; }
	}
}
