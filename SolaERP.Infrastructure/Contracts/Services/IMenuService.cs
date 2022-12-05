using SolaERP.Infrastructure.Entities.Menu;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IMenuService
    {
        Task<List<Menu>> GetUserMenusWithChildAsync();
    }
}
