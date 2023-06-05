using SolaERP.Application.Dtos.ApproveStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class ApprovalStageGetModel
    {
        public int ApproveStageMainId { get; set; }
        public int ProcedureId { get; set; }
        public object ProcedureName { get; set; }
        public object ApproveStageName { get; set; }
        public object ApproveStageCode { get; set; }
        public bool ReApproveOnChange { get; set; }
        public object CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ApproveStagesDetailDto> MyProperty { get; set; }
    }
}
