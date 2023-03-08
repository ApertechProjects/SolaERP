using System.Data;

namespace SolaERP.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbCommand CreateCommand();
        Task SaveChangesAsync();
        void SaveChanges();

    }
}
