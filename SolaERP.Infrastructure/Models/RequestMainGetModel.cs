using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Models
{
    public class RequestMainGetModel
    {
        public int BusinessUnitId { get; set; }
        public List<string> ItemCodes { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ApproveStatuses ApproveStatus { get; set; }
        public Statuss Status { get; set; }
    }
}
