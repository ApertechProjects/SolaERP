using SolaERP.Infrastructure.Entities.Layout;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ILayoutRepository
    {
        Task<Layout> GetUserLayoutAsync(int userId, string layoutKey);
        Task<bool> SaveLayoutAsync(Layout layout);
        Task<bool> DeleteLayoutAsync(Layout layout);
    }
}
