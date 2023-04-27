using SolaERP.Application.Entities.Layout;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ILayoutRepository
    {
        Task<Layout> GetUserLayoutAsync(int userId, string layoutKey);
        Task<bool> SaveLayoutAsync(Layout layout);
        Task<bool> DeleteLayoutAsync(int userId, string layoutKey);
    }
}
