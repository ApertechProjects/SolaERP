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
        Task AddUserToGroupOrDeleteAsync(UserToGroupModel model);
        Task<bool> AddMenuToGroupOrDeleteAsync(GroupMenuIDSaveModel saveOrDeleteModel);
        Task AddBusiessUnitToGroupOrDeleteAsync(int groupId, int busiessUnitId);
        Task AddApproveRoleToGroupOrDelete(int groupId, int approveRoleId);
        Task<bool> DeleteBuyerByGroupIdAsync(int groupBuyerId);
        public Task<List<GroupBuyer>> GetBuyersByGroupIdAsync(int groupId);
        Task<bool> SaveBuyerByGroupAsync(GroupBuyerSaveModel model);
        Task<List<GroupAnalysisCode>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<bool> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId);
        Task<bool> SaveAnalysisCodeByGroupAsync(AnalysisCodeSaveModel model);
        Task<List<GroupRole>> GetGroupRolesAsync(int groupId);
        Task<bool> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model);
        Task<bool> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId);
    }
}
