using SolaERP.Application.Attributes;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class DueDiligenceGrid : BaseEntity
    {
        [DbColumn("DueDiligenceGridDataId")]
        public int Id { get; set; }
        [DbColumn("DueDiligenceDesignId")]
        public int DueDesignId { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public int Type { get; set; }
    }
}
