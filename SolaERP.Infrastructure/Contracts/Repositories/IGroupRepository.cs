using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Models;
using System.Data;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IGroupRepository
    {
        public Task<List<GroupAdditionalPrivilage>> GetAdditionalPrivilegesForGroupAsync(int groupId);
        public Task<bool> AdditionalPrivilegeAddOrUpdateAsync(GroupAdditionalPrivilage additionalPrivilage);
        Task<List<Groups>> GetAllAsync();
        Task<int> AddUpdateOrDeleteGroupAsync(int userID, Groups entity);
        Task AddUserToGroupAsync(DataTable model, int groupId);
        Task DeleteUserToGroupAsync(DataTable model, int groupId);
        Task AddBusinessUnitsToGroupAsync(DataTable model, int groupId);
        Task DeleteBusinessUnitsFromGroupAsync(DataTable model, int groupId);
        Task<bool> AddMenuToGroupOrDeleteAsync(GroupMenuIDSaveModel saveOrDeleteModel);
        Task AddBusinessUnitToGroupOrDeleteAsync(int groupId, int busiessUnitId);
        Task AddApproveRoleToGroupOrDelete(int groupId, int approveRoleId);
        Task<bool> DeleteBuyerByGroupIdAsync(int groupBuyerId);
        public Task<List<GroupBuyer>> GetBuyersByGroupIdAsync(int groupId);
        Task<bool> SaveBuyerByGroupAsync(GroupBuyerSaveModel model);
        Task<List<GroupAnalysisCode>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<bool> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId);
        Task<bool> SaveAnalysisCodeByGroupAsync(AnalysisCodeSaveModel model);
        Task<List<GroupRole>> GetGroupRolesByGroupIdAsync(int groupId);
        Task<bool> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model);
        Task<bool> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId);
        Task<List<GroupUser>> GetGroupsByUserIdAsync(int userId);
        Task<List<GroupEmailNotification>> GetGroupEmailNotificationsAsync(int groupId);
        Task<bool> CreateEmailNotificationAsync(CreateGroupEmailNotificationModel model);
        Task<bool> UpdateEmailNotificationAsync(GroupEmailNotification entity);
        Task<bool> DeleteEmailNotificationAsync(int groupEmailNotificationId);
        Task<Group> GetGroupInfoAsync(int groupId);
    }
}
