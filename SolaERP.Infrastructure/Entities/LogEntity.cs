using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities
{
    public class LogEntity : BaseEntity
    {
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
