using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.ApproveStages
{
    public class ApproveStagesDetailDto
    {
        public int ApproveStageDetailsId { get; set; }
        public int ApproveStageMainId { get; set; }
        public string ApproveStageDetailsName { get; set; }
        public int Sequence { get; set; }
    }
}
