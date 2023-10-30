using SolaERP.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Invoice
{
    public class InvoiceRegisterDetail : BaseEntity
    {
        public int InvoiceMatchingDetailId { get; set; }
        public int InvoiceMatchingMainId { get; set; }
        public long LineNo { get; set; }
        public string OrderNo { get; set; }
        public object OrderLine { get; set; }
        public decimal Quantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string MWP { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public decimal BaseRate { get; set; }
        public decimal ReportingRate { get; set; }
        public decimal BaseTotal { get; set; }
        public decimal ReportingTotal { get; set; }
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
