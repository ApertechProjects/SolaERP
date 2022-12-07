using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IMenuService
    {
        Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildsAsync(string finderToken);
        Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilagesAsync(string finderToken);
    }
}
