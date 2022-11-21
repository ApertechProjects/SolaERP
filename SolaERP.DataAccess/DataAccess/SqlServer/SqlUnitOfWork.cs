using SolaERP.Infrastructure.UnitOfWork;
using System.Data;

namespace SolaERP.DataAccess.DataAcces.SqlServer
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public SqlUnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();

        }

        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();

            if (_transaction == null)
                _transaction = _connection.BeginTransaction();

            command.Transaction = _transaction;
            return command;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction have already been commited. Check your Transaction handling.");
            _transaction.Commit();
            _transaction = null;
        }

        public async Task SaveChangesAsync()
        {
            await Task.Run(SaveChanges);
        }
    }
}
