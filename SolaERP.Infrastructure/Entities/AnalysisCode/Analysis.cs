using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.AnalysisCode
{
    public class Analysis : ModifyEntity
    {
        public int AnalysisCodesId { get; set; }
        public int AnalysisDimensionId { get; set; }
        public string AnalysisCode { get; set; }
        public string AnalysisName { get; set; }
        public string Description { get; set; }
        public string AdditionalDescription { get; set; }
        public string AdditionalDescription2 { get; set; }
        public int Status { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public bool IsLinked { get; set; }
    }
}
