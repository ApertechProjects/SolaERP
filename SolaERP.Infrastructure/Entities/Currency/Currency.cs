using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Currency
{
    public class Currency : BaseEntity
    {
        public string CurrCode { get; set; }
        public string Description { get; set; }
    }
}
