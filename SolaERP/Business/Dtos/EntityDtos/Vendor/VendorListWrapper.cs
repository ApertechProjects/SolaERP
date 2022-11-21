using SolaERP.Business.Dtos.EntityDtos.ApproveStage;

namespace SolaERP.Business.Dtos.EntityDtos.Vendor
{
    public class VendorListWrapper
    {
        public List<VendorWFA> WFAVendor { get; set; }
        public List<VendorAll> AllVendor { get; set; }
        public List<ApproveStage_List> ApproveStages { get; set; }
        public List<VendorDraft> VendorDrafts { get; set; }
    }
}
