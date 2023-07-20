using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Models
{
    public class BaseFilterModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
