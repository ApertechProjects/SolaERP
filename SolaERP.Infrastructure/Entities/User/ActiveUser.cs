using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.User
{
    public class ActiveUser : BaseEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}
