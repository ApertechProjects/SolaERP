using SolaERP.Application.Entities.ApproveStage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class ApproveStageMainInputModel : ApproveStagesMain
    {
        public string BusinessUnitId { get; set; }
    }
}
