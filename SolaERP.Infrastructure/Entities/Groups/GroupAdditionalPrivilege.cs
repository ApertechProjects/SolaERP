namespace SolaERP.Application.Entities.Groups
{
    public class GroupAdditionalPrivilege : BaseEntity
    {
        public int GroupAdditionalPrivilegeId { get; set; }
        public int AdditionalPrivilegeId { get; set; }
        public string AdditionalPrivilegeName { get; set; }
        public bool IsInGroup { get; set; }
    }
}
