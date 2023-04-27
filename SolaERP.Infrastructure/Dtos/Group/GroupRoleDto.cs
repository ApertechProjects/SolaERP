namespace SolaERP.Application.Dtos.Group
{
    public class GroupRoleDto
    {
        public int GroupApproveRoleId { get; set; }
        public int GroupId { get; set; }
        public int ApproveRoleId { get; set; }
        public string ApproveRoleName { get; set; }
        public bool IsInGroup { get; set; }
    }
}
