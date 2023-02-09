using SolaERP.Infrastructure.Entities.Menu;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IMenuRepository : ICrudOperations<Menu>
    {
        Task<List<MenuWithPrivilages>> GetUserMenuWithPrivillagesAsync(int userId);
        Task<List<GroupMenu>> GetGroupMenusByGroupIdAsync(int groupId);
    }
}
