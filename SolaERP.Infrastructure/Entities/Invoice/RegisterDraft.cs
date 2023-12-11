using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
    public class RegisterDraft : BaseEntity
    {
        public int InvoiceRegisterId { get; set; }
        public long LineNo { get; set; }
        public int InvoiceType { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public string InvoiceNo { get; set; }
        public int OrderTypeId { get; set; }
        public string OrderType { get; set; }
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public string ReferenceDocNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string LineDescription { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public DateTime DueDate { get; set; }
        public int AgingDays { get; set; }
        public bool HasAttachments { get; set; }
    }
}
