using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IReturnableAddAsync<T> where T : BaseEntity
    {
        Task<int> AddAsync(List<T> entities);
        Task<int> AddAsync(T entity);
        bool Remove(int Id);
    }
}
