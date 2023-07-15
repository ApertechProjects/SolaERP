
namespace SolaERP.Application.Dtos.AnalysisCode
{
    public class AnalysisDto : ModifyEntity
    {
        public int AnalysisCodesId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public string AnalysisDimensionCode { get; set; }
        public int AnalysisDimensionId { get; set; }
        public string AnalysisCode { get; set; }
        public string AnalysisName { get; set; }
        public string Description { get; set; }
        public string AdditionalDescription { get; set; }
        public string AdditionalDescription2 { get; set; }
        public int Status { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public int LinkedAnalysisDimensionid { get; set; }
        public bool IsLinked { get; set; }
    }
}
