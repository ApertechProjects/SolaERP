using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities.AnalysisCode
{
    public class AnalysisCode : BaseEntity
    {
        public int AnalysisCodesId { get; set; }

        [DbColumn("AnalysisCode")]
        public string AnalyticCode { get; set; }
        public string AnalysisName { get; set; }
    }
}
