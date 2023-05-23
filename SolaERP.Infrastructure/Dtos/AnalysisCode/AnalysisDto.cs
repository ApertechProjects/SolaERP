using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.AnalysisCode
{
    public class AnalysisDto
    {
        public int AnalysisCodesId { get; set; }
        public int AnalysisDimensionId { get; set; }
        public string AnalysisCode { get; set; }
        public string AnalysisName { get; set; }
        public string Description { get; set; }
    }
}
