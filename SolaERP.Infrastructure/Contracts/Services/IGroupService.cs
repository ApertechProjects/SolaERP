using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.Group;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IGroupService
    {
        Task<ApiResponse<List<GroupsDto>>> GetAllAsync();
        Task<ApiResponse<bool>> SaveGroupAsync(string name, GroupSaveModel model);
        Task<ApiResponse<List<GroupAdditionalPrivilegeDto>>> GetAdditionalPrivilegesForGroupAsync(int groupId);
        Task<ApiResponse<List<GroupBuyerDto>>> GetBuyersByGroupIdAsync(int groupId);
        Task<ApiResponse<bool>> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId);
        Task<ApiResponse<List<GroupAnalysisCodeDto>>> GetAnalysisCodesByGroupIdAsync(int groupId);
        Task<ApiResponse<bool>> AddAnalysisCodeAsync(int groupId, List<int> analysisIds);
        Task<ApiResponse<List<GroupRoleDto>>> GetGroupRolesByGroupIdAsync(int groupId);
        Task<ApiResponse<bool>> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model);
        Task<ApiResponse<bool>> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId);
        Task<ApiResponse<List<GroupUserDto>>> GetGroupsByUserIdAsync(int userId);
        Task<ApiResponse<List<GroupsDto>>> GetUserGroupsWithoutCurrents(int id);
        Task<ApiResponse<List<GroupEmailNotification>>> GetGroupEmailNotificationsAsync(int groupId);
        Task AddUsersAsync(List<int> users, int groupId);
        Task DeleteUsersAsync(List<int> users, int groupId);
        Task AddBusinessUnitsAsync(List<int> users, int groupId);
        Task DeleteBusinessUnitsAsync(List<int> users, int groupId);
        Task<ApiResponse<GroupDto>> GetGroupInfoAsync(int groupId);


    }
}
