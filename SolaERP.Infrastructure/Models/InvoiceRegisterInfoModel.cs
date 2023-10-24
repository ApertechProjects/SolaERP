using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class InvoiceRegisterInfoModel
    {
        public int InvoiceRegisterId { get; set; }
        public string GRNs { get; set; }
        public int BusinessUnitId { get; set; }
    }
}
