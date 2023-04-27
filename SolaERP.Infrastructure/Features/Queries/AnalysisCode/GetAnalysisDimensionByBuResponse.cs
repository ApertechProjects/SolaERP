namespace SolaERP.Application.Features.Queries.AnalysisCode
{
    public class GetAnalysisDimensionByBuResponse
    {
        public int Id { get; set; }
        public string AnalysisDimensionCode { get; set; }
        public string AnalysisDimensionName { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnit { get; set; }

    }
}
