using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Bid
{
    public class BidIUDResponse
    {
        public int Id { get; set; }
        public string BidNo { get; set; }
        public List<int> BidDetailIds { get; set; }
    }
}
