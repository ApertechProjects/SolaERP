using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.SupplierEvaluation
{
    public class Score : BaseEntity
    {
        public string Data { get; set; }
        public decimal Value { get; set; }
    }
}
