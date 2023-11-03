

namespace SolaERP.Application.Entities.Vendors
{
    public class VendorAll : BaseVendor
    {
        public string CountryCode { get; set; }
        public string ApproveStatusName { get; set; }
        public string VendorType { get; set; }

    }

    public class VendorDraft : BaseVendor
    {
        public string CountryCode { get; set; }
        public string ApproveStatusName { get; set; }

    }

    public class VendorApproved : BaseVendor
    {
        public string CountryCode { get; set; }
        public string ApproveStatusName { get; set; }

    }
}
