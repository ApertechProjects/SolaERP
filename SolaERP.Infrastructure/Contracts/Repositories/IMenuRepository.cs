using SolaERP.Application.Entities.Menu;
using SolaERP.Application.Entities.User;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IMenuRepository : ICrudOperations<Menu>
    {
        Task<List<MenuWithPrivilages>> GetUserMenuWithPrivilegesAsync(int userId);
        Task<List<GroupMenu>> GetGroupMenusByGroupIdAsync(int groupId);
        Task<AdditionalPrivilegeAccess> GetAdditionalPrivilegeAccessAsync(int userId);
        Task<List<MenuWithPrivilages>> GetMenuWithPrivilegesAsync(int groupId);
    }
}
