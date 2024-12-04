namespace SolaERP.Application.Entities.Invoice;

public class InvoiceMatchingDetail : BaseEntity
{
	public int @InvoiceMatchingMainid { get; set; }
	public List<InvoicesMatchingDetailsType> InvoicesMatchingDetailsTypeList { get; set; }
}

public class InvoicesMatchingDetailsType
{
	public int? InvoiceMatchingDetailId { get; set; }
	public int LineNo { get; set; }
	public string OrderNo { get; set; }
	public int OrderLine { get; set; }
	public string GRNNo { get; set; }
	public int GRNLine { get; set; }
	public string MWP { get; set; }
	public decimal Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal Total { get; set; }
	public string Description { get; set; }
	public int AnalysisCode1Id { get; set; }
	public int AnalysisCode2Id { get; set; }
	public int AnalysisCode3Id { get; set; }
	public int AnalysisCode4Id { get; set; }
	public int AnalysisCode5Id { get; set; }
	public int AnalysisCode6Id { get; set; }
	public int AnalysisCode7Id { get; set; }
	public int AnalysisCode8Id { get; set; }
	public int AnalysisCode9Id { get; set; }
	public int AnalysisCode10Id { get; set; }
	public decimal AdvanceTotal { get; set; }
	public string AccountCode { get; set; }
	public string ItemCode { get; set; }
	public decimal? BaseTotal { get; set; }
	public decimal? ReportingTotal { get; set; }
	public decimal? WithHoldingTaxAmount { get; set; }
	public decimal? BaseWithHoldingTaxAmount { get; set; }
	public decimal? ReportingWithHoldingTaxAmount { get; set; }
	public decimal? VATAmount { get; set; }
	public decimal? BaseVATAmount { get; set; }
	public decimal? ReportingVATAmount { get; set; }
	public decimal? BaseAdvanceTotal { get; set; }
	public decimal? ReportingAdvanceTotal { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal GrossAmount { get; set; }

}

public class InvoicesMatchingDetailsTypeDto
{
	public int? InvoiceMatchingDetailId { get; set; }
	public int LineNo { get; set; }
	public string OrderNo { get; set; }
	public int OrderLine { get; set; }
	public string GRNNo { get; set; }
	public int GRNLine { get; set; }
	public string MWP { get; set; }
	public decimal ServiceAmount { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal InvoiceAmount { get; set; }
	public string Description { get; set; }
	public int AnalysisCode1Id { get; set; }
	public int AnalysisCode2Id { get; set; }
	public int AnalysisCode3Id { get; set; }
	public int AnalysisCode4Id { get; set; }
	public int AnalysisCode5Id { get; set; }
	public int AnalysisCode6Id { get; set; }
	public int AnalysisCode7Id { get; set; }
	public int AnalysisCode8Id { get; set; }
	public int AnalysisCode9Id { get; set; }
	public int AnalysisCode10Id { get; set; }
	public decimal AdvanceAmount { get; set; }
	public string AccountCode { get; set; }
	public string ItemCode { get; set; }
	public decimal? BaseTotal { get; set; }
	public decimal? ReportingTotal { get; set; }
	public decimal? WithHoldingTaxAmount { get; set; }
	public decimal? BaseWithHoldingTaxAmount { get; set; }
	public decimal? ReportingWithHoldingTaxAmount { get; set; }
	public decimal? VATAmount { get; set; }
	public decimal? BaseVATAmount { get; set; }
	public decimal? ReportingVATAmount { get; set; }
	public decimal? BaseAdvanceTotal { get; set; }
	public decimal? ReportingAdvanceTotal { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal GrossAmount { get; set; }
}