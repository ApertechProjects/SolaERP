using System.Data;

namespace SolaERP.Application.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbCommand CreateCommand();
        Task SaveChangesAsync();
        void SaveChanges();

    }
}
