using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class InvoiceGetDetailsModel
    {
        public int BusinessUnitId { get; set; }
        public int InvoiceRegisterId { get; set; }
        public List<string> GRNs { get; set; }
    }
}
