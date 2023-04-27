using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Item_Code
{
    public class ItemCodeInfoDto
    {
        public string ItemName1 { get; set; }
        public string ItemName2 { get; set; }
        public string UnitOfPurch { get; set; }
        public decimal AvailableInMainStock { get; set; }
    }
}
