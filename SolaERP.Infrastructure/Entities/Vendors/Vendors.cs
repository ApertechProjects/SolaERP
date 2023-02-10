using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Entities.Vendors
{
    public class Vendors : BaseEntity
    {
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string TaxId { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string RepresentedProducts { get; set; }
        public string RepresentedCompanies { get; set; }
        public string PaymentTerms { get; set; }
        public int CreditDays { get; set; }
        public int V60DaysPayment { get; set; } //db name 60DaysPayment
        public string Country { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string ApproveStatusName { get; set; }
        public int BusinessUnitId { get; set; }
        public string OtherProducts { get; set; }
        public int ApproveStageMainId { get; set; }
        public string CompanyAddress { get; set; }
        public DateTime CompanyRegistrationDate { get; set; }
        public string TaxOffice { get; set; }
        public int UserId { get; set; }
        public string UserStatusName { get; set; }
        public VendorApprovals VendorApprovals { get; set; }
        public SolaERP.Infrastructure.Entities.Auth.User AppUser { get; set; }
    }
}
