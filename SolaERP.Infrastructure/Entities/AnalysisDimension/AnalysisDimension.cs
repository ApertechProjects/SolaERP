namespace SolaERP.Application.Entities.AnalysisDimension
{
    public class AnalysisDimension : BaseEntity
    {
        public int AnalysisDimensionId { get; set; }
        public string AnalysisDimensionCode { get; set; }
        public string AnalysisDimensionName { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public int BusinessUnitId { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
