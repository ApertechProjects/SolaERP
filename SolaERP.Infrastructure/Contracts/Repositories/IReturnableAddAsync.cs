using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IReturnableAddAsync<T> where T : BaseEntity
    {
        Task<int> AddRangeAsync(List<T> entities);
        Task<int> AddAsync(T entity);
        Task<bool> RemoveAsync(int Id);
    }
}
