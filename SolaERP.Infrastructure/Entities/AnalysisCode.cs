using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities
{
    public class AnalysisCode : BaseEntity
    {
        public int AnalysisCodeId { get; set; }

        [DbColumn("AnalysisCode")]
        public string AnalyticCode { get; set; }
        public string AnalysisName { get; set; }
    }
}
