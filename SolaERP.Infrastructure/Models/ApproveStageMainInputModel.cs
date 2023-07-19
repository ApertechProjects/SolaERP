using SolaERP.Application.Entities.ApproveStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class ApproveStageMainInputModel
    {
        public int ApproveStageMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public int ProcedureId { get; set; }
        public string ProcedureKey { get; set; }
        public string ApproveStageName { get; set; }
        public string ApproveStageCode { get; set; }
        public bool ReApproveOnChange { get; set; }
    }
}
