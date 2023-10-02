using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Payment
{
    public class InfoHeader : BaseEntity
    {
        public int PaymentDocumentMainId { get; set; }
        public int BusinessUnitId { get; set; }
        public string Reference { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
        public int OrderAdvance { get; set; }
        public int PaymentDocumentTypeId { get; set; }
        public int PaymentDocumentPriorityId { get; set; }
        public int Status { get; set; }
        public int ApproveStatus { get; set; }
        public int PaymentStatus { get; set; }
        public int ApproveStageMainId { get; set; }
        public string PaymentRequestNo { get; set; }
        public DateTime PaymentRequestDate { get; set; }
        public DateTime SentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int HasRequestAttach { get; set; }
        public int HasRFQAttach { get; set; }
        public int HasBidAttach { get; set; }
        public int HasBCCAttach { get; set; }
        public int HasOrderAttach { get; set; }
        public int HasInvoiceAttach { get; set; }
    }
}
