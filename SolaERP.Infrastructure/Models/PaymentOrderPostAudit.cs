using SolaERP.Application.Dtos.Payment;

namespace SolaERP.Application.Models
{
    public class PaymentOrderPostAudit
    {
        public int JournalNo { get; set; }
        public string SunUser { get; set; }
        public int CurrentPeriod { get; set; }
        public int AllocationReference { get; set; }
        public List<ASalfldgDto> ASalfldgs { get; set; }
        public List<AllocationDataDto> AllocationDatas { get; set; }
        public List<ASalfldgLadDto> ASalfldgLads { get; set; }
    }
}
