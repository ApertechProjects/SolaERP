using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class InfoApproval : BaseEntity
    {
        public int Sequence { get; set; }
        public string UserPhoto { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public DateTime ApproveDate { get; set; }
        public int ApproveStatus { get; set; }
        public string SignaturePhoto { get; set; }
        public string Comment { get; set; }
    }
}
