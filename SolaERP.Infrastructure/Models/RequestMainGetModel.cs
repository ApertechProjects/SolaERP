namespace SolaERP.Infrastructure.Models
{
    public class RequestMainGetModel
    {
        public int BusinessUnitId { get; set; }
        public List<string> ItemCodes { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int[] ApproveStatus { get; set; }
        public int[] Status { get; set; }
    }
}
