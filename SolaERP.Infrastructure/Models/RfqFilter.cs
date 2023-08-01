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
        public Status Status { get; set; } = Status.All;
    }

    public class RFQFilterBase
    {
        public int BusinessUnitId { get; set; } = 2;
        public string ItemCode { get; set; }
        public Emergency Emergency { get; set; } = Emergency.All;
        public RfqType RFQType { get; set; } = RfqType.All;
        public ProcurementType ProcurementType { get; set; } = ProcurementType.All;
    }
}
