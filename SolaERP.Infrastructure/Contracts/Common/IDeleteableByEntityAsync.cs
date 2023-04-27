using SolaERP.Application.Entities;

namespace SolaERP.Application.Contracts.Common
{
    public interface IDeleteableByEntityAsync<T> where T : BaseEntity
    {
        bool DeleteAsync(T entity);
    }
}
