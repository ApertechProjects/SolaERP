using SolaERP.Application.Entities.AnalysisCode;
using SolaERP.Application.Entities.BusinessUnits;
using SolaERP.Application.Entities.Buyer;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Models;
using System.Data;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IGroupRepository
    {
        Task<List<GroupAdditionalPrivilege>> GetAdditionalPrivilegesAsync(int groupId);
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
        Task<List<GroupBuyer>> GetBuyersAsync(int groupId);
        Task<List<GroupAnalysisCode>> GetAnalysisCodesAsync(int groupId);
        Task<bool> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId);
        Task<bool> AddAnalysisCodeAsync(int groupId, DataTable table);
        Task<List<GroupRole>> GetGroupRolesAsync(int groupId);
        Task<bool> SaveGroupRoleAsync(GroupRoleSaveModel model);
        Task<bool> DeleteGroupRoleAsync(int groupApproveRoleId);
        Task<List<GroupUser>> GetUserGroupsAsync(int userId);
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
        Task<List<BusinessUnitForGroup>> GetGroupBusinessUnitsAsync(int groupId);
        Task<int> GetGroupIdByVendorAdmin();
    }
}
