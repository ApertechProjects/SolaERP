using System.Numerics;

namespace SolaERP.Application.Dtos.RFQ
{
    public class RFQDeadlineFinishedMailForBuyerDto
    {
        public int RFQMainId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public string RFQNo { get; set; }
        public string BusinessUnitName { get; set; }
        public DateTime RFQDeadline { get; set; }
        
        public int BusinessUnitId { get; set; }
    }
}