using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class CreateOrderFromBidDto
    {
        public int BidMainId { get; set; }
        public int UserId { get; set; }
    }
}
