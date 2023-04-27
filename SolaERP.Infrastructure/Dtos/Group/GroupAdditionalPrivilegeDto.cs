namespace SolaERP.Application.Dtos.Group
{
    public class GroupAdditionalPrivilegeDto
    {
        public int GroupAdditionalPrivilegeId { get; set; }
        public int AdditionalPrivilegeId { get; set; }
        public string AdditionalPrivilegeName { get; set; }
        public bool IsInGroup { get; set; }
    }
}
