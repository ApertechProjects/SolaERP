using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.ApproveRole
{
    public class ApproveRoleDto
    {
        public int ApproveRoleId { get; set; }
        public string ApproveRoleName { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BusinessUnitId { get; set; }
    }
}
