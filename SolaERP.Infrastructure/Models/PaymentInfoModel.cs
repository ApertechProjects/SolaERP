using SolaERP.Application.Dtos.Payment;
using SolaERP.Application.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class PaymentInfoModel
    {
        public InfoHeaderDto Header { get; set; }
        public List<InfoDetailDto> Detail { get; set; }
        public List<InfoApproval> Approval { get; set; }
        public List<AttachmentDto> AttachmentTypes { get; set; }
        public PaymentLink PaymentLink { get; set; }
    }

    public class PaymentLink
    {
        public List<RequestLink> RequestLinks { get; set; }
        public List<RFQLink> RFQLinks { get; set; }
        public List<BidLink> BidLinks { get; set; }
        public List<BidComparisonLink> BidComparisonLinks { get; set; }
        public List<OrderLink> OrderLinks { get; set; }
        public List<InvoiceLink> InvoiceLinks { get; set; }
    }

    public class InvoiceLink
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class OrderLink
    {
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
    }

    public class BidComparisonLink
    {
        public int BidComparisonId { get; set; }
        public string ComparisonNo { get; set; }
    }

    public class BidLink
    {
        public int BidMainId { get; set; }
        public string BidNo { get; set; }
    }

    public class RFQLink
    {
        public int RFQMainId { get; set; }
        public string RFQNo { get; set; }
    }

    public class RequestLink
    {
        public int RequestMainId { get; set; }
        public string RequestNo { get; set; }
    }
}

