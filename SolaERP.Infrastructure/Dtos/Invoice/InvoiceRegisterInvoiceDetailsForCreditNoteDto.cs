namespace SolaERP.Application.Dtos.Invoice;

public class InvoiceRegisterInvoiceDetailsForCreditNoteDto
{
    public string LineDescription { get; set; }
    public decimal QTY { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal Amount { get; set; }
    public decimal WithHoldingAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal BaseAmount { get; set; }
    public decimal ReportingAmount { get; set; }
    public decimal BaseTaxAmount { get; set; }
    public decimal ReportingTaxAmount { get; set; }
    public decimal BaseGrossAmount { get; set; }
    public decimal ReportingGrossAmount { get; set; }
    public decimal WithHoldingTaxAmount { get; set; }
    public decimal BaseWithHoldingTaxAmount { get; set; }
    public decimal ReportingWithHoldingTaxAmount { get; set; }
    public string AccountCode { get; set; }
    public string FixedAssetCode { get; set; }
    public string ANAL_T0 { get; set; }
    public string ANAL_T1 { get; set; }
    public string ANAL_T2 { get; set; }
    public string ANAL_T3 { get; set; }
    public string ANAL_T4 { get; set; }
    public string ANAL_T5 { get; set; }
    public string ANAL_T6 { get; set; }
    public string ANAL_T7 { get; set; }
    public string ANAL_T8 { get; set; }
    public int? AnalysisCode1Id { get; set; }
    public int? AnalysisCode2Id { get; set; }
    public int? AnalysisCode3Id { get; set; }
    public int? AnalysisCode4Id { get; set; }
    public int? AnalysisCode5Id { get; set; }
    public int? AnalysisCode6Id { get; set; }
    public int? AnalysisCode7Id { get; set; }
    public int? AnalysisCode8Id { get; set; }
    public int? AnalysisCode9Id { get; set; }
    public int? AnalysisCode10Id { get; set; }
}