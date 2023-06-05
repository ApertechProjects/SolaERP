using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.AnalysisStructure
{
    public class AnalysisStructureWithBuDto : AnalysisStructureDto
    {
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string ProcedureName { get; set; }
    }
}
