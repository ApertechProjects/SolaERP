using SolaERP.Application.Entities;

namespace SolaERP.Application.Contracts.Common
{
    public interface IReadableAsync<T> where T : BaseEntity
    {
        public Task<List<T>> GetAllAsync();
    }
}
