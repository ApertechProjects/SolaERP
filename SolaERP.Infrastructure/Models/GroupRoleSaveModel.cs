using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Models
{
    public class GroupRoleSaveModel
    {
        public int GroupApproveRoleId { get; set; }
        public int GroupId { get; set; }
        public int ApproveRoleId { get; set; }
    }
}
