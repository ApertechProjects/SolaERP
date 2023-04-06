using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.Buyer
{
    public class GroupBuyerDto
    {
        public int GroupBuyerId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
    }
}
