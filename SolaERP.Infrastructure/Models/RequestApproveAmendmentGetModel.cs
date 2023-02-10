namespace SolaERP.Infrastructure.Models
{
    public class RequestApproveAmendmentGetModel
    {
        public int BusinessUnitId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ItemCode { get; set; }
    }
}
