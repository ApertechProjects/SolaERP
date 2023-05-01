namespace SolaERP.Application.Dtos.AnalysisCode
{
    public class GroupAnalysisCodeDto
    {
        public int GroupAnalysisCodeId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public int AnalysisDimensionId { get; set; }
        public string AnalysisDimensionCode { get; set; }
        public int AnalysisCodesId { get; set; }
        public string AnalysisCode { get; set; }
        public string AnalysisName { get; set; }
    }
}
