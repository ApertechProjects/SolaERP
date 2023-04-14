using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Models
{
    public class AddUserToGroupModel
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
