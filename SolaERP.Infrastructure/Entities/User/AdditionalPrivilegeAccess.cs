namespace SolaERP.Application.Entities.User
{
    public class AdditionalPrivilegeAccess : BaseEntity
    {
        public bool VendorDraft { get; set; }
        public bool RequestAttachment { get; set; }
        public bool RequestSendToApprove { get; set; }
    }
}
