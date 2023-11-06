namespace SolaERP.Application.Models
{
    public class RequestMainGetModel
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public List<string> ItemCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string[] ApproveStatus { get; set; }
        public int[] Status { get; set; }
    }
}
