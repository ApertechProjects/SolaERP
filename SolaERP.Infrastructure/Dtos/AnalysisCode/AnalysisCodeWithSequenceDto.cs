using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.AnalysisCode
{
    public class AnalysisCodeWithSequenceDto
    {
        public int Sequence { get; set; }
        public List<AnalysisCodeDto> AnalysisCodes { get; set; }
    }
}
