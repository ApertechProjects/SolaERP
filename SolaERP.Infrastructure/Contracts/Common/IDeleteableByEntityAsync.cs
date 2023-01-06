using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IDeleteableByEntityAsync<T> where T : BaseEntity
    {
        bool DeleteAsync(T entity);
    }
}
