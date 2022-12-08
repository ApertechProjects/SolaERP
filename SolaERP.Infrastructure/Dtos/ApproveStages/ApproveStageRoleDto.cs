using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.ApproveStages
{
    public class ApproveStageRoleDto
    {
        public int ApproveStageRoleId { get; set; }
        public int ApproveStageDetailId { get; set; }
        public int ApproveRoleId { get; set; }
        public ApproveRole.ApproveRoleDto ApproveRolesDto { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
    }
}
