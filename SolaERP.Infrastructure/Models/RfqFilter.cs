using SolaERP.Application.Enums;

namespace SolaERP.Application.Models
{
    public class RfqFilter : RFQFilterBase
    {
        public DateTime DateFrom { get; set; } = DateTime.Now.AddMonths(-3);
        public DateTime DateTo { get; set; } = DateTime.Now;

    }

    public class RfqAllFilter : RfqFilter
    {
        //public List<Status> Status { get; set; }
        public string Status { get; set; }
    }

    public class RFQFilterBase
    {
        public int BusinessUnitId { get; set; } = 2;
        public string ItemCode { get; set; }
        //public List<Emergency> Emergency { get; set; }
        public string Emergency { get; set; }

        public string RFQType { get; set; }     
        //public RfqType RFQType { get; set; } = RfqType.All;
        public string ProcurementType { get; set; }
        //public List<ProcurementType> ProcurementType { get; set; }
    }
}
