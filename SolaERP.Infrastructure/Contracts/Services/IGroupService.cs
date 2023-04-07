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
    }
}
