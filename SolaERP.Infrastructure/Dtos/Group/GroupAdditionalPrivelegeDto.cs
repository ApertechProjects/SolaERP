namespace SolaERP.Infrastructure.Dtos.Group
{
    public class GroupAdditionalPrivelegeDto
    {
        public int GroupAdditionalPrivilegeId { get; set; }
        public int GroupId { get; set; }
        public int VendorDraft { get; set; }
        public int RequestAttachment { get; set; }
        public int RequestSendToApprove { get; set; }
    }
}
