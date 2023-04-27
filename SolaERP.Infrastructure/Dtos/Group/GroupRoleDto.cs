namespace SolaERP.Infrastructure.Dtos.Group
{
    public class GroupRoleDto
    {
        public int GroupApproveRoleId { get; set; }
        public int GroupId { get; set; }
        public int ApproveRoleId { get; set; }
        public string ApproveRoleName { get; set; }
        public string ApproveRole { get; set; }
        public bool IsInGroup { get; set; }
    }
}
