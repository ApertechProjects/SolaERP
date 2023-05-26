namespace SolaERP.Application.Entities.AnalysisDimension
{
    public class BuAnalysisDimension : ModifyEntity
    {
        public int AnalysisDimensionId { get; set; }
        public string AnalysisDimensionCode { get; set; }
        public string AnalysisDimensionName { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
