using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IReadableAsync<T> where T : BaseEntity
    {
        public Task<List<T>> GetAllAsync();
    }
}
