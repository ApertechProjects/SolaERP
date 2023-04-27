namespace SolaERP.Application.Models
{
    public class RequestMainDraftModel
    {
        public int BusinessUnitId { get; set; }
        public string BusinessUnitCode { get; set; }
        public List<string> ItemCodes { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
