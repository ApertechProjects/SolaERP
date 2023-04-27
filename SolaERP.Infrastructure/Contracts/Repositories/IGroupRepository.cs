namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IGroupRepository
    {
        public Task<List<GroupAdditionalPrivilage>> GetAdditionalPrivilegesForGroupAsync(int groupId);
        Task<int> AddUpdateOrDeleteGroupAsync(int userID, Groups entity);
        Task<List<Groups>> GetAllAsync();
        Task AddUsersAsync(DataTable model, int groupId);
        Task DeleteUsersAsync(DataTable model, int groupId);
        Task AddBusinessUnitsAsync(DataTable model, int groupId);
        Task DeleteBusinessUnitsAsync(DataTable model, int groupId);
        Task<bool> AddMenuToGroupOrDeleteAsync(GroupMenuIDSaveModel saveOrDeleteModel);
        Task<bool> DeleteBuyerByGroupIdAsync(int groupBuyerId);
        public Task<List<GroupBuyer>> GetBuyersByGroupIdAsync(int groupId);
        Task<List<GroupAnalysisCode>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<bool> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId);
        Task<bool> AddAnalysisCodeAsync(int groupId, DataTable table);
        Task<List<GroupRole>> GetGroupRolesByGroupIdAsync(int groupId);
        Task<bool> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model);
        Task<bool> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId);
        Task<List<GroupUser>> GetGroupsByUserIdAsync(int userId);
        Task<List<GroupEmailNotification>> GetGroupEmailNotificationsAsync(int groupId);
        Task<bool> CreateEmailNotificationAsync(CreateGroupEmailNotificationModel model);
        Task<bool> UpdateEmailNotificationAsync(GroupEmailNotification entity);
        Task<bool> DeleteEmailNotificationAsync(int groupEmailNotificationId);
        Task<Group> GetGroupInfoAsync(int groupId);
        Task DeleteApproveRolesFromGroupAsync(DataTable data, int groupId);
        Task AddApproveRolesToGroupAsync(DataTable data, int groupId);
        Task DeleteAdditionalPrivilegesAsync(DataTable data, int groupId);
        Task AddAdditionalPrivilegesAsync(DataTable data, int groupId);
        Task AddBuyersAsync(DataTable data, int groupId);
        Task DeleteBuyersAsync(DataTable data, int groupId);
    }
}
