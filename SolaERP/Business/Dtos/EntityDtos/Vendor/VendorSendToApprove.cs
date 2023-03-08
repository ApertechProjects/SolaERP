namespace SolaERP.Business.Dtos.EntityDtos.Vendor
{
    public class VendorSendToApprove
    {
        public List<int> VendorsId { get; set; }
        public int ApproveStageMainId { get; set; }
    }
}
