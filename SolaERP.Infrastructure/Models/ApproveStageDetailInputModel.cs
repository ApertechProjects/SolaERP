using SolaERP.Application.Dtos.ApproveStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class ApproveStageDetailInputModel : ApproveStagesDetailDto
    {
        public List<ApproveStageRoleDto> ApproveStageRoles { get; set; }
    }
}
