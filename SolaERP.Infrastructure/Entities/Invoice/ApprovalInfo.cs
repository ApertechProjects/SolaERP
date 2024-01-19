using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
    public class ApprovalInfo : BaseEntity
    {
        public int InvoiceRegisterApprovalId { get; set; }
        public int Sequence { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int UserId { get; set; }
        public string ApprovedBy { get; set; }
        public string Comment { get; set; }
        public int ApproveStatus { get; set; }
        public string ApproveStatusName { get; set; }
    }
}
