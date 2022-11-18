using SolaERP.Infrastructure.Entities;

namespace SolaERP.DataAccess.Abstract
{
    public interface ICrudOperations<Tentity> where Tentity : BaseEntity
    {
        List<Tentity> GetAllAsync();
        Task<Tentity> GetByIdAsync(int id);
        bool Add(Tentity entity);
        void Update(Tentity entity);
        void Remove(Tentity entity);
    }
}
