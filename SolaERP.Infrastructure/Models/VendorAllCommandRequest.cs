using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class VendorAllCommandRequest
    {
        public VendorFilter Filter { get; set; }
        public Enums.Status Status { get; set; }
        public ApprovalStatus Approval { get; set; }
    }
}
