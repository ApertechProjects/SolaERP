using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Bid
{
    public class BidRFQListLoadDto
    {
        public int RFQMainId { get; set; }
        public string RFQNo { get; set; }
        public DateTime RFQDeadline { get; set; }
        public string Emergency { get; set; }
        public string Buyer { get; set; }
        public string BusinessCategoryName { get; set; }
        public int BidCount { get; set; }
    }
}
