using SolaERP.Infrastructure.Entities.Menu;
using SolaERP.Infrastructure.Entities.User;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IMenuRepository : ICrudOperations<Menu>
    {
        Task<List<MenuWithPrivilages>> GetUserMenuWithPrivilegesAsync(int userId);
        Task<List<GroupMenu>> GetGroupMenusByGroupIdAsync(int groupId);
        Task<AdditionalPrivilegeAccess> GetAdditionalPrivilegeAccessAsync(int userId);
    }
}
