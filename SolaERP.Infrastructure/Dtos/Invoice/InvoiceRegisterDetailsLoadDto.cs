using SolaERP.Application.Entities;

namespace SolaERP.Application.Dtos.Invoice
{
    public class InvoiceRegisterDetailForPO : InvoiceRegisterDetailForGeneral
    {
        public string GRNNo { get; set; }
        public int GRNLine { get; set; }
        public decimal GRNQTY { get; set; }
        public decimal Total { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal ReportingAmount { get; set; }
        public DateTime GRNDate { get; set; }
    }

    public class InvoiceRegisterDetailForOther : InvoiceRegisterDetailForGeneral
    {
        public string OrderNo { get; set; }
        public decimal OrderQTY { get; set; }
        public string LineDescription { get; set; }
        public string MWP { get; set; }
        public decimal Quantity { get; set; }
        public decimal ServiceAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal BaseRate { get; set; }
        public decimal ReportingRate { get; set; }
        public decimal BaseTotal { get; set; }
        public decimal ReportingTotal { get; set; }
    }

    public class InvoiceRegisterDetailForGeneral : BaseEntity
    {
        public int? InvoiceMatchingDetailId { get; set; }
        public int? InvoiceMatchingMainId { get; set; }
        public int? AdvanceInvoiceRegisterId { get; set; }
        public long LineNo { get; set; }
        public int OrderLine { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public decimal AdvanceAmount { get; set; }
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
    }
}
