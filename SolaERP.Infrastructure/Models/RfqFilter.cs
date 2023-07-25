using SolaERP.Application.Enums;

namespace SolaERP.Application.Models
{
    public class RfqFilter
    {
        public int BusinessUnitId { get; set; }
        public string ItemCode { get; set; }
        public Emergency Emergency { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string RFQType { get; set; }
        public ProcurementType ProcurementType { get; set; }
    }

    public class RfqAllFilter : RfqFilter
    {
        public Status Status { get; set; }
    }
}
