using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IReturnableRepoMethodAsync<T> where T : BaseEntity
    {
        public Task<int> AddOrUpdateAsync(T entity);
    }
}
