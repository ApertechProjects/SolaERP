using SolaERP.Application.Dtos.AnalysisCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class AnalysisCodesModel
    {
        public int AnalysisDimensionId { get; set; }
        public string AnalysisDimensionCode { get; set; }
        public List<AnalysisListDto> AnalysisCodes { get; set; }
    }
}
