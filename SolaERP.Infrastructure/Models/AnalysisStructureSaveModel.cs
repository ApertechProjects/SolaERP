
using System.Text.Json.Serialization;

namespace SolaERP.Application.Models
{
    public class AnalysisStructureSaveModel
    {
        public int? BusinessUnitId { get; set; }
        public int? ProcedureId { get; set; }
        public int? CatId { get; set; }
        public int? AnalysisDimensionid1 { get; set; }
        public int? AnalysisDimensionid2 { get; set; }
        public int? AnalysisDimensionid3 { get; set; }
        public int? AnalysisDimensionid4 { get; set; }
        public int? AnalysisDimensionid5 { get; set; }
        public int? AnalysisDimensionid6 { get; set; }
        public int? AnalysisDimensionid7 { get; set; }
        public int? AnalysisDimensionid8 { get; set; }
        public int? AnalysisDimensionid9 { get; set; }
        public int? AnalysisDimensionid10 { get; set; }

        [JsonIgnore]
        public int? UserId { get; set; }
    }
}
