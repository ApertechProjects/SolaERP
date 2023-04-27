using SolaERP.Application.Attributes;

namespace SolaERP.Application.Dtos.AnalysisCode
{
    public class AnalysisCodeDto
    {
        public int AnalysisCodesId { get; set; }
        public string AnalyticCode { get; set; }
        public string AnalysisName { get; set; }
        public int Sequence { get; set; }
    }
}
