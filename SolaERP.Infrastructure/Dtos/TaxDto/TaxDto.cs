using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.TaxDto
{
    public class TaxDto
    {
        public int TaxId { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public decimal Tax { get; set; }
    }
}
