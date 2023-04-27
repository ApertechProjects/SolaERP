using SolaERP.Application.Entities;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface ICrudOperations<Tentity> where Tentity : BaseEntity
    {
        Task<List<Tentity>> GetAllAsync();
        Task<Tentity> GetByIdAsync(int id);
        Task<bool> AddAsync(Tentity entity);
        Task UpdateAsync(Tentity entity);
        Task<bool> RemoveAsync(int Id);

    }
}
