namespace SolaERP.Infrastructure.Models
{
    public class RequestApproveAmendmentModel
    {
        public int BusinessUnitId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<string> ItemCodes { get; set; }
    }
}
