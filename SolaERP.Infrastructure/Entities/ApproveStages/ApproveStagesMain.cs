using SolaERP.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.ApproveStage
{
    public class ApproveStagesMain : BaseEntity
    {
        public int ApproveStageMainId { get; set; }
        public int ProcedureId { get; set; }
        public Procedure.Procedure Procedure { get; set; }
        [DbIgnore]
        public int BusinessUnitId { get; set; }
        public string ApproveStageName { get; set; }
    }
}
