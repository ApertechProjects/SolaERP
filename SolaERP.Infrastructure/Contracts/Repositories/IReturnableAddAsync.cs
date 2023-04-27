using SolaERP.Application.Entities;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IReturnableAddAsync<T> where T : BaseEntity
    {
        Task<int> AddAsync(T entity);
        Task<bool> RemoveAsync(int Id);
    }
}
