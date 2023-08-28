using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Bid
{
    public class BidDisqualifyDto
    {
        public int BidMainId { get; set; }
        public int Discualified { get; set; }
        public string DiscualificationReason { get; set; }
    }
}
