namespace SolaERP.Infrastructure.Dtos.Request
{
    public class RequestApproveAmendmentGetParametersDto
    {
        public string FinderToken { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ItemCode { get; set; }
    }
}
