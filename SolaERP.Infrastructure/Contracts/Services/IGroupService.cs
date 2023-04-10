using SolaERP.Infrastructure.Dtos.AnalysisCode;
using SolaERP.Infrastructure.Dtos.AnaysisDimension;
using SolaERP.Infrastructure.Dtos.Buyer;
using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IGroupService
    {
        Task<ApiResponse<List<GroupsDto>>> GetAllAsync();
        Task<ApiResponse<bool>> SaveGroupAsync(string finderToken, GroupSaveModel model);
        Task<ApiResponse<List<GroupAdditionalPrivilage>>> GetAdditionalPrivilegesForGroupAsync(int groupId);
        Task<ApiResponse<bool>> AddOrUpdateAsync(GroupAdditionalPrivelegeDto additionalPrivilage);
        Task<ApiResponse<bool>> DeleteBuyerByGroupIdAsync(int groupBuyerId);
        Task<ApiResponse<List<GroupBuyerDto>>> GetBuyersByGroupIdAsync(int groupId);
        Task<ApiResponse<bool>> SaveBuyerByGroupAsync(GroupBuyerSaveModel model);
        Task<ApiResponse<bool>> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId);
        Task<ApiResponse<List<GroupAnalysisCodeDto>>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<ApiResponse<bool>> SaveAnalysisCodeByGroupAsync(AnalysisCodeSaveModel model);
        Task<ApiResponse<List<GroupRoleDto>>> GetGroupRolesAsync(int groupId);
        Task<ApiResponse<bool>> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model);
        Task<ApiResponse<bool>> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId);
        Task<ApiResponse<List<GroupUserDto>>> GetGroupsByUserIdAsync(int userId);
    }
}
