using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.UserReport
{
    public class UserReportList : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
