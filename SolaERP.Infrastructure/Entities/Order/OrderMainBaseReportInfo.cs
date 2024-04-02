using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Order
{
    public class OrderMainBaseReportInfo : BaseEntity
    {
        public int OrderMainId { get; set; }
        public string OrderNo { get; set; }
        public string Currency { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime Date { get; set; }
    }
}
