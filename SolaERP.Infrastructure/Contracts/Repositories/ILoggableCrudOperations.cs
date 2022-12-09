using SolaERP.Infrastructure.Entities;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface ILoggableCrudOperations<Tentity> where Tentity : BaseEntity
    {
        Task<bool> AddAsync(Tentity entity, int userId = default);
        void Update(Tentity entity);
    }
}
