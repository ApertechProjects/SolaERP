using SolaERP.Application.Entities;

namespace SolaERP.Application.Contracts.Common
{
    public interface IReturnableRepoMethodAsync<T> where T : BaseEntity
    {
        public Task<int> AddOrUpdateAsync(T entity);
    }
}
