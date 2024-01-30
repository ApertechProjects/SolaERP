using SolaERP.Application.Dtos.Invoice;
using SolaERP.Application.Entities.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class InvoiceMatchResultModelDto
    {
        public InvoiceMatchMainDataDto InvoiceMatchMainData { get; set; }
        public List<InvoiceMatchDetailDataDto> InvoiceMatchDetailDatas { get; set; }
        public List<InvoiceMatchAdvanceDto> InvoiceMatchAdvances { get; set; }
        public List<InvoiceMatchGRNDto> InvoiceMatchGRN { get; set; }
    }

    public class InvoiceMatchResultModel
    {
        public InvoiceMatchMainData InvoiceMatchMainData { get; set; }
        public List<InvoiceMatchDetailData> InvoiceMatchDetailDatas { get; set; }
        public List<InvoiceMatchAdvance> InvoiceMatchAdvances { get; set; }
        public List<InvoiceMatchGRN> InvoiceMatchGRN { get; set; }
    }
}
