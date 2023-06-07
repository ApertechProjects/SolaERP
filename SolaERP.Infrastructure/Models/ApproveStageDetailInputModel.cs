using SolaERP.Application.Dtos.ApproveStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class ApproveStageDetailInputModel 
    {
        public int ApproveStageDetailsId { get; set; }
        public int ApproveStageMainId { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
        public bool Skip { get; set; }
        public int SkipDays { get; set; }
        public bool BackToInitiatorOnReject { get; set; }
        public List<ApproveStageRoleDto> ApproveStageRoles { get; set; }
        public string Type { get; set; }
    }
}
