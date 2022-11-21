namespace SolaERP.Business.Dtos.Wrappers
{
    public class GroupSaveWrapper
    {
        public int GroupId { get; set; }
        public List<int> BusinessUnitIds { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public List<int> Users { get; set; }
        public List<int> ApproveRoles { get; set; }
        public MenuGroupWRP Menus { get; set; }
        public AdditionalPrivilegeWRP AdditionalPrivilege { get; set; }
    }
}
