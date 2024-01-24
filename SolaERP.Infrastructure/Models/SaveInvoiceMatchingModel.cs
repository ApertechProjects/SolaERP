using SolaERP.Application.Entities.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class SaveInvoiceMatchingModel
    {
        public InvoiceMathcingMain Main { get; set; }
        public List<InvoicesMatchingDetailsTypeDto> Details { get; set; }
        public List<AdvanceInvoicesMatchingType> AdvanceInvoicesMatchingTypeList { get; set; }
    }

    public class SaveInvoiceMatchingGRNModel
    {
        public InvoiceMathcingMain Main { get; set; }
        public List<InvoicesMatchingDetailsTypeDto> Details { get; set; }
        public List<AdvanceInvoicesMatchingType> AdvanceInvoicesMatchingTypeList { get; set; }
        public List<RNEInvoicesMatchingType> RNEInvoicesMatchingTypeList { get; set; }
    }
}
