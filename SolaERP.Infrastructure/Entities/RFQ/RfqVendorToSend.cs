namespace SolaERP.Application.Entities.RFQ
{
    public class RfqVendorToSend : BaseEntity
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public int RFQMainId { get; set; }
        public DateTime? RFQDeadline { get; set; }
        public string HasApproval { get; set; }
        public string RFQNo { get; set; }
    }
}