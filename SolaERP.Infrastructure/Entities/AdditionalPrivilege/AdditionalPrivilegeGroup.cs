using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.AdditionalPrivilege
{
    public class AdditionalPrivilegeGroup : BaseEntity
    {
        public int GroupAdditionalPrivilegeId { get; set; }
        public int GroupId { get; set; }
        public int VendorDraft { get; set; }
    }
}
