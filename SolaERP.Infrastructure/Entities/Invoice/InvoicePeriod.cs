using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Invoice
{
    public class InvoicePeriod : BaseEntity
    {
        public string periodFrom { get; set; }
        public string periodTo { get; set; }
    }
}
