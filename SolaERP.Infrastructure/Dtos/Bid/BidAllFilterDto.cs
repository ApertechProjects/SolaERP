using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Bid
{
    public class BidAllFilterDto
    {
        public int BusinessUnitId { get; set; }
        public List<string> ItemCode { get; set; }
        public List<Emergency> Emergency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<Enums.Status> Status { get; set; }
        public List<ApprovalStatus> ApproveStatus { get; set; }
    }
}
