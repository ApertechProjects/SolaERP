using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class VendorsApprovals : BaseEntity
    {
        public int VendorApprovalId { get; set; }
        public int VendorId { get; set; }
        public int Sequence { get; set; }
        public int ApproveStageDetailsid { get; set; }
        public int UserId { get; set; }
        public int ApproveStatus { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Comment { get; set; }
    }
}
