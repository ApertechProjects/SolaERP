using SolaERP.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.UOM
{
    public class UOM : BaseEntity
    {
        [DbColumn("UOM")]
        public string UOM_Name { get; set; }
        public string Description { get; set; }
    }
}
