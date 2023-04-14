using SolaERP.Infrastructure.Entities.AnalysisCode;
using SolaERP.Infrastructure.Entities.Buyer;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IGroupRepository
    {
        public Task<List<GroupAdditionalPrivilage>> GetAdditionalPrivilegesForGroupAsync(int groupId);
        public Task<bool> AdditionalPrivilegeAddOrUpdateAsync(GroupAdditionalPrivilage additionalPrivilage);
        Task<List<Groups>> GetAllAsync();
        Task<int> AddUpdateOrDeleteGroupAsync(int userID, Groups entity);
        Task<bool> AddUserToGroupOrDeleteAsync(UserToGroupModel model);
        Task<bool> AddMenuToGroupOrDeleteAsync(GroupMenuIDSaveModel saveOrDeleteModel);
        Task AddBusiessUnitToGroupOrDeleteAsync(int groupId, int busiessUnitId);
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
        Task<List<GroupEmailNotfication>> GetGroupEmailNotficationsAsync(int groupId);
        Task<bool> CreateEmailNotficationAsync(CreateGroupEmailNotficationModel model);
        Task<bool> UpdateEmailNotficationAsync(GroupEmailNotfication entity);
        Task<bool> DeleteEmailNotficationAsync(int groupEmailNotficationId);
        Task<bool> AddUserToGroupAsync(AddUserToGroupModel model);
        Task<bool> DeleteUserFromGroupAsync(int groupUserId);
    }
}
