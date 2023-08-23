using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.BidComparison
{
    public class BidComparisonDraftFilterDto
    {
        public int BusinessUnitid { get; set; }
        public List<Emergency> Emergency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
