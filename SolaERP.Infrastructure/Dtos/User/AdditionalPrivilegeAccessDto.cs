using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.User
{
    public class AdditionalPrivilegeAccessDto
    {
        public bool VendorDraft { get; set; }
        public bool RequestAttachment { get; set; }
        public bool RequestSendToApprove { get; set; }
    }
}
