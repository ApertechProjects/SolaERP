using SolaERP.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.ApproveStage
{
    public class ApproveStagesMainDto
    {
        public int ApproveStageMainId { get; set; }
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public string ApproveStageName { get; set; }
    }
}
