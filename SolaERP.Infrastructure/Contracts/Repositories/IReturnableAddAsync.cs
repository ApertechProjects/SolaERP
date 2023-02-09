using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IReturnableAddAsync<T> where T : BaseEntity
    {
        Task<int> AddAsync(T entity);
        Task<bool> RemoveAsync(int Id);
    }
}
