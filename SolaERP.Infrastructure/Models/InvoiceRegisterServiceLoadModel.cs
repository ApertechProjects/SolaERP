using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class InvoiceRegisterServiceLoadModel
    {
        public int BusinessUnitId { get; set; }
        public int OrderMainId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
    }


    public class InvoiceRegisterGRNLoadModel
    {
        public int BusinessUnitId { get; set; }
        public List<string> Grns { get; set; }
        public int OrderMainId { get; set; }
        public DateTime Date { get; set; }
    }
}
