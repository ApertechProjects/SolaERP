using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.SupplierEvaluation
{
    public class PrequalificationMandatory
    {
        public int DueDiligenceDesignId { get; set; }
        public bool IsMandatory { get; set; }
    }
}
