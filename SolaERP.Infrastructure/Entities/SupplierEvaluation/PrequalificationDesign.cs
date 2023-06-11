
namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class PrequalificationDesign : BaseEntity
    {
        public int PrequalificationDesignId { get; set; }
        public int VendorPrequalificationCategoryId { get; set; }
        public int LineNo { get; set; }
        public string Discipline { get; set; }
        public string Questions { get; set; }
        public decimal HasTextbox { get; set; }
        public decimal HasTextarea { get; set; }
        public decimal HasCheckbox { get; set; }
        public decimal HasRadiobox { get; set; }
        public decimal HasInt { get; set; }
        public decimal HasDecimal { get; set; }
        public decimal HasDateTime { get; set; }
        public decimal HasAttachment { get; set; }
        public decimal HasList { get; set; }
        public string Title { get; set; }
        public int Weight { get; set; }
        public decimal HasGrid { get; set; }
        public int? GridRowLimit { get; set; }
        public int? GridColumnCount { get; set; }
        public string? Column1Alias { get; set; }
        public string? Column2Alias { get; set; }
        public string? Column3Alias { get; set; }
        public string? Column4Alias { get; set; }
        public string? Column5Alias { get; set; }
    }
}
