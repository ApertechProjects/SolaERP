using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Buyer
{
    public class Buyer : BaseEntity
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
    }
}
