namespace SolaERP.Infrastructure.Models
{
    public class RequestMainGetModel
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public List<string> ItemCodes { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string[] ApproveStatus { get; set; }
        public string[] Status { get; set; }
    }
}
