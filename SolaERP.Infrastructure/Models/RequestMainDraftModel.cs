namespace SolaERP.Infrastructure.Models
{
    public class RequestMainDraftModel
    {
        public int BusinessUnitId { get; set; }
        public string ItemCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
