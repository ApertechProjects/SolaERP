using System.Data;

namespace SolaERP.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDbCommand CreateCommand();
        Task SaveChangesAsync();
        void SaveChanges();

    }
}
