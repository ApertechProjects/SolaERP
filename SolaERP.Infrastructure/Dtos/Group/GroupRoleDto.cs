using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.Group
{
    public class GroupRoleDto
    {
        public int GroupApproveRoleId { get; set; }
        public int GroupId { get; set; }
        public int ApproveRoleId { get; set; }
        public string ApproveRoleName { get; set; }
    }
}
