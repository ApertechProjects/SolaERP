using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Request
{
    public class RequestCategory:BaseEntity
    {
        public int CatId { get; set; }
        public string Name { get; set; }
    }
}
