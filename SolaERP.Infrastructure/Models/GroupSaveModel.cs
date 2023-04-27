namespace SolaERP.Infrastructure.Models
{
    public class GroupSaveModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        #region Users
        public List<int> AddUsers { get; set; }
        public List<int> RemoveUsers { get; set; }
        #endregion
        #region BusinessUnitUnits
        public List<int> AddBusinessUnits { get; set; }
        public List<int> RemoveBusinessUnits { get; set; }
        #endregion
        #region ApproveRoles
        public List<int> AddApproveRoles { get; set; }
        public List<int> RemoveApproveRoles { get; set; }
        #endregion
        #region AdditionalPrivileges
        public List<int> AddAdditionalPrivileges { get; set; }
        public List<int> RemoveAdditionalPrivileges { get; set; }
        #endregion
        public GroupMenuPrivilegeListModel Menus { get; set; }
        public GroupAdditionalPrivilegeModel AdditionalPrivilege { get; set; }
    }
}
