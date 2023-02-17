namespace SolaERP.Infrastructure.Models
{
    public class GroupSaveModel
    {
        public int GroupId { get; set; }
        public List<int> BusinessUnitIds { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public List<int> Users { get; set; }
        public List<int> ApproveRoles { get; set; }
        public GroupMenuPrivilegeListModel Menus { get; set; }
        public GroupAdditionalPrivilegeModel AdditionalPrivilege { get; set; }
    }
}
