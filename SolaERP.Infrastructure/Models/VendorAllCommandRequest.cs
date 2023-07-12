using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class VendorAllCommandRequest : VendorFilter
    {
        public List<int> Status { get; set; }
        public List<int> Approval { get; set; }
    }

    public class VendorSendToApproveRequest
    {
        public List<int> VendorIds { get; set; }
    }
}
