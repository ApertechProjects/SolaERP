using System.Data;

namespace SolaERP.DataAccess.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IDbCommand CreateCommand();
        Task SaveChangesAsync();
        void SaveChanges();

    }
}
