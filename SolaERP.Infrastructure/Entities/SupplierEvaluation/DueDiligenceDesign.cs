
namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class DueDiligenceDesign
    {
        public string Title { get; set; }
        public int DesignId { get; set; }
        public int LineNo { get; set; }
        public string Question { get; set; }
        public decimal HasTextBox { get; set; }
        public decimal HasCheckBox { get; set; }
        public decimal HasRadioBox { get; set; }
        public decimal HasInt { get; set; }
        public decimal HasDecimal { get; set; }
        public decimal HasDateTime { get; set; }
        public decimal HasAttachment { get; set; }
        public decimal HasBankList { get; set; }
        public decimal HasTexArea { get; set; }
        public bool ParentCompanies { get; set; }
        public decimal HasGrid { get; set; }
        public int GridRowLimit { get; set; }
        public int GridColumnCount { get; set; }
        public bool HasAgreement { get; set; }
        public string AgreementText { get; set; }
        public string Column1Alias { get; set; }
        public string Column2Alias { get; set; }
        public string Column3Alias { get; set; }
        public string Column4Alias { get; set; }
        public string Column5Alias { get; set; }
        public decimal Weight { get; set; }
    }
}
