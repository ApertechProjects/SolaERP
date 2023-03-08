namespace SolaERP.Business.Dtos.EntityDtos.Vendor
{
    public class VendorWFAModel
    {
        public int Sequence { get; set; }
        public int VendorId { get; set; }
        public int ApproveStatus { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
