using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestMainGetParametersDto
    {
        public int BusinessUnitId { get; set; }
        public string ItemCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ApproveStatuses ApproveStatus { get; set; }
        public Status Status { get; set; }

    }
}
