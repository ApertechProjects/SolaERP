using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IMenuService
    {
        Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildsAsync(string finderToken);
        Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilagesAsync(string finderToken);
        Task<ApiResponse<GroupMenuResponseDto>> GetGroupMenuWithPrivillageListByGroupIdAsync(string finderToken, int groupId);
        Task<ApiResponse<AdditionalPrivilegeAccessDto>> GetAdditionalPrivilegeAccessAsync(string authToken);
    }
}
