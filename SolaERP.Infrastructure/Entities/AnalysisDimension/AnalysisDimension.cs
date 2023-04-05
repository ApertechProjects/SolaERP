using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.AnalysisDimension
{
    public class AnalysisDimension : BaseEntity
    {
        public int AnalysisDimensionId { get; set; }
        public string AnalysisDimensionCode { get; set; }
        public string AnalysisDimensionName { get; set; }
    }
}
