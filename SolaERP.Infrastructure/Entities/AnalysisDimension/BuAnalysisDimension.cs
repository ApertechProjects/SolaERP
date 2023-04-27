namespace SolaERP.Application.Entities.AnalysisDimension
{
    public class BuAnalysisDimension : AnalysisDimension
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
    }
}
