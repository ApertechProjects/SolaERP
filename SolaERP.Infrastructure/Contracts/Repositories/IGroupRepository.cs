using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Models;
using System.Data;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IGroupRepository
    {
        Task<List<GroupAdditionalPrivilegeDto>> GetAdditionalPrivilegesAsync(int groupId);
        Task<int> AddUpdateOrDeleteGroupAsync(int userID, Groups entity);
        Task<List<Groups>> GetAllAsync();
        Task AddUsersAsync(DataTable model, int groupId);
        Task<bool> DeleteAnalysisCodeAsync(int groupId, DataTable table);
        Task DeleteUsersAsync(DataTable model, int groupId);
        Task AddBusinessUnitsAsync(DataTable model, int groupId);
        Task DeleteBusinessUnitsAsync(DataTable model, int groupId);
        Task<bool> AddMenuAsync(int groupId, DataTable table);
        Task<bool> DeleteMenuAsync(int groupId, DataTable table);
        Task<bool> DeleteBuyerByGroupIdAsync(int groupBuyerId);
        Task<List<GroupBuyer>> GetBuyersByGroupIdAsync(int groupId);
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
        Task<Group> GetGroupInfoAsync(int groupId);
        Task DeleteApproveRolesFromGroupAsync(DataTable data, int groupId);
        Task AddApproveRolesToGroupAsync(DataTable data, int groupId);
        Task DeleteAdditionalPrivilegesAsync(DataTable data, int groupId);
        Task AddAdditionalPrivilegesAsync(DataTable data, int groupId);
        Task AddBuyersAsync(DataTable data, int groupId);
        Task DeleteBuyersAsync(DataTable data, int groupId);
        Task DeleteEmailNotificationAsync(DataTable data, int groupId);
        Task AddEmailNotificationsAsync(DataTable data, int groupId);
    }
}
