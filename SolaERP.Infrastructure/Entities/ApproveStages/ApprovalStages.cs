using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.ApproveStages
{
    public class ApprovalStages : BaseEntity
    {
        public int ApproveStageMainId { get; set; }
        public string ApproveStageName { get; set; }
    }
}
