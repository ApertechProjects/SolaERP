using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Bid
{
    public class BidDisqualify
    {
        public int BidMainId { get; set; }
        public int Discualified { get; set; }
        public string DiscualificationReason { get; set; }
        public int UserId { get; set; }
    }
}
