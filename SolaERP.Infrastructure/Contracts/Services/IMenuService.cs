using SolaERP.Infrastructure.Dtos.Menu;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IMenuService
    {
        Task<ApiResponse<List<ParentMenuDto>>> GetUserMenusWithChildsAsync();
        Task<ApiResponse<List<MenuWithPrivilagesDto>>> GetUserMenusWithPrivilagesAsync();
    }
}
