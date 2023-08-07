using SolaERP.Application.Dtos.Supplier;
using SolaERP.Application.Dtos.Venndors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BidComparison
{
    public class BidDto
    {
        public int Id { get; set; }
        public VendorDto Vendor { get; set; }
        public List<BidComparisonDetailDto> Details { get; set; }
        public List<UserAcceptanceDto> UserAcceptances { get; set; }
    }
}
