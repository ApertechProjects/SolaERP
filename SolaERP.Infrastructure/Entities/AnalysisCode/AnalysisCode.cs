using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.AnalysisCode
{
    public class AnalysisCode : BaseEntity
    {
        public int AnalysisCodesId { get; set; }

        [DbColumn("AnalysisCode")]
        public string AnalyticCode { get; set; }
        public string AnalysisName { get; set; }
        public int Sequence { get; set; }
    }
}
