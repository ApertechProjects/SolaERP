using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Location
{
    public class Location : BaseEntity
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}
