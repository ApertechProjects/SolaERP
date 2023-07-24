using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.RFQ
{
    public class RfqDraft
    {
        public int RFQMainId { get; set; }
        public DateTime RequiredOnSiteDate { get; set; }
        public string Emergency { get; set; }
        public DateTime RFQDate { get; set; }
        public string RFQType { get; set; }
        public int RFQNo { get; set; }
        public DateTime DesiredDeliveryDate { get; set; }
        public string ProcurementType { get; set; }
        public string OtherReasons { get; set; }
        public DateTime SentDate { get; set; }
        public string Comment { get; set; }
        public DateTime RFQDeadline { get; set; }
        public string Buyer { get; set; }
        public bool SingleUnitPrice { get; set; }
        public string PlaceOfDelivery { get; set; }
        public int BusinessCategoryid { get; set; }
    }
}
