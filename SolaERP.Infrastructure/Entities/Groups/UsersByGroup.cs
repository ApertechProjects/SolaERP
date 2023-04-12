using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Groups
{
    public class UsersByGroup : BaseEntity
    {
        public int GroupUserId { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}
