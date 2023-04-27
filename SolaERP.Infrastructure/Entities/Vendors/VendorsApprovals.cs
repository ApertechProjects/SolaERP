namespace SolaERP.Application.Entities.Vendors
{
    public class VendorApprovals : BaseEntity
    {
        public int VendorApprovalId { get; set; }
        public int VendorId { get; set; }
        public int Sequence { get; set; }
        public int ApproveStageDetailsid { get; set; }
        public int UserId { get; set; }
        public int ApproveStatus { get; set; }
        public DateTime ApproveDate { get; set; }
        public string Comment { get; set; }
    }
}
