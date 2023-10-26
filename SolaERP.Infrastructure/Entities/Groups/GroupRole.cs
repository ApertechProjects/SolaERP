namespace SolaERP.Application.Entities.Groups
{
    public class GroupRole : BaseEntity
    {
        public int GroupApproveRoleId { get; set; }
        public int GroupId { get; set; }
        public string BusinessUnitCode { get; set; }
        public int ApproveRoleId { get; set; }
        public string ApproveRoleName { get; set; }
        public bool IsInGroup { get; set; }
    }
}
