using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class AnalysisCodeModel : BaseEntity
    {
        public int CatId { get; set; }
        public List<AnalysisCode> AnalysisCodes { get; set; }
        //public Analysisis Analysis { get; set; }
    }

    public class Analysisis
    {
        public AnalysisCode1 AnalysisCode1 { get; set; }
        public AnalysisCode2 AnalysisCode2 { get; set; }
        public AnalysisCode3 AnalysisCode3 { get; set; }
        public AnalysisCode4 AnalysisCode4 { get; set; }
        public AnalysisCode5 AnalysisCode5 { get; set; }
        public AnalysisCode6 AnalysisCode6 { get; set; }
        public AnalysisCode7 AnalysisCode7 { get; set; }
        public AnalysisCode8 AnalysisCode8 { get; set; }
        public AnalysisCode9 AnalysisCode9 { get; set; }
        public AnalysisCode10 AnalysisCode10 { get; set; }
    }


}

