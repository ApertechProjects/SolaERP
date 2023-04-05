using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Models
{
    public class AnalysisCodeSaveModel
    {
        public int GroupAnalysisCodeId { get; set; }
        public int GroupId { get; set; }
        public int BusinessUnitId { get; set; }
        public int AnalysisDimensionId { get; set; }
        public int AnalysisCodesId { get; set; }
    }
}
