using SolaERP.Application.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.ApproveStage
{
    public class ApproveStagesMainDto
    {
        public int ApproveStageMainId { get; set; }
        public int ProcedureId { get; set; }
        public Procedure.ProcedureDto Procedure { get; set; }
        public int BusinessUnitId { get; set; }
        public string ApproveStageName { get; set; }
    }
}
