using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.AnalysisCode
{
    public class AnalysisCodesDto
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public string AnalysisCode { get; set; }
        public string AnalysisName { get; set; }
    }
}
