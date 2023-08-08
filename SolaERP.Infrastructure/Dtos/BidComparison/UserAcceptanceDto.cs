using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class UserAcceptanceDto
    {
        public int BidId { get; set; }
        public int UserId { get; set; }
        public bool IsAccepted { get; set; }
    }
}
