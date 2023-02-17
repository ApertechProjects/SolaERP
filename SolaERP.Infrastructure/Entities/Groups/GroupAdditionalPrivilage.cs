namespace SolaERP.Infrastructure.Entities.Groups
{
    public class GroupAdditionalPrivilage : BaseEntity
    {
        public int GroupAdditionalPrivilegeId { get; set; }
        public int GroupId { get; set; }
        public int VendorDraft { get; set; }
        public int RequestAttachment { get; set; }
        public int RequestSendToApprove { get; set; }
    }
}
