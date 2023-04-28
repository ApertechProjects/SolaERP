using SolaERP.Application.Dtos.AnalysisCode;
using SolaERP.Application.Dtos.BusinessUnit;
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
        Task<ApiResponse<List<GroupAdditionalPrivilegeDto>>> GetAdditionalPrivilegesAsync(int groupId);
        Task<ApiResponse<List<GroupBuyerDto>>> GetGroupBuyersAsync(int groupId);
        Task<ApiResponse<bool>> DeleteAnalysisCodeAsync(int groupAnalysisCodeId);
        Task<ApiResponse<List<GroupAnalysisCodeDto>>> GetAnalysisCodesAsync(int groupId);
        Task<ApiResponse<bool>> AddAnalysisCodeAsync(int groupId, List<int> analysisIds);
        Task<ApiResponse<List<GroupRoleDto>>> GetGroupRolesAsync(int groupId);
        Task<ApiResponse<bool>> SaveGroupRoleAsync(GroupRoleSaveModel model);
        Task<ApiResponse<bool>> DeleteGroupRoleAsync(int groupApproveRoleId);
        Task<ApiResponse<List<GroupUserDto>>> GetUserGroupsAsync(int userId);
        Task<ApiResponse<List<GroupsDto>>> GetUserGroupsWithoutCurrents(int id);
        Task<ApiResponse<List<GroupEmailNotification>>> GetGroupEmailNotificationsAsync(int groupId);
        Task AddUsersAsync(List<int> users, int groupId);
        Task DeleteUsersAsync(List<int> users, int groupId);
        Task AddBusinessUnitsAsync(List<int> users, int groupId);
        Task DeleteBusinessUnitsAsync(List<int> users, int groupId);
        Task<ApiResponse<GroupDto>> GetGroupInfoAsync(int groupId);
        Task<ApiResponse<List<BusinessUnitForGroupDto>>> GetGroupBusinessUnitsAsync(int groupId);
        Task<ApiResponse<bool>> DeleteGroupAsync(string identity, int groupId);
    }
}
