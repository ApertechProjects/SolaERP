using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class DeliveryTerms : BaseEntity
    {
        public int DeliveryTermId { get; set; }
        public string DeliveryTermCode { get; set; }
        public string DeliveryTermName { get; set; }
    }
}
