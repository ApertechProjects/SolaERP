using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Mail
{
	public class RequestMailDto
	{
		public string BusinessUnitName { get; set; }
		public string RequesterEmail { get; set; }
		public string BuyerEmail { get; set; }
	}
}
