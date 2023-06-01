using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.User
{
    public class AdditionalPrivilegeAccessDto
    {
        public int GroupAdditionalPrivilegeId { get; set; }
        public int GroupId { get; set; }
        public int AdditionalPrivelegeId { get; set; }
        public string AdditionalPrivelegeName { get; set; }
    }
}
