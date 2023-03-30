using SolaERP.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.User
{
    public class ActiveUser : BaseEntity
    {
        [DbColumn("Id")]
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}
