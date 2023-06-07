using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.AnaysisDimension
{
    public class DimensionCheckDto
    {
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string ProcedureName { get; set; }
        public int CatId { get; set; }
    }
}
