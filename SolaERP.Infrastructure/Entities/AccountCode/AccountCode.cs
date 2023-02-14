using SolaERP.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Account
{
    public class AccountCode : BaseEntity
    {
        [DbColumn("AccountCode")]
        public string Account_Code { get; set; }
        public string Description { get; set; }
    }
}
