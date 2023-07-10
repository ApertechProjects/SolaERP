using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Vendors
{
    public class WithHoldingTaxData : BaseEntity
    {
        public int WithHoldingTaxId { get; set; }
        public string WithHoldingTaxCode { get; set; }
        public string WithHoldingTaxName { get; set; }
        public decimal WithHoldingTax { get; set; }
    }
}
