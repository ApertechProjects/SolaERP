using SolaERP.DataAccess.Abstract;
using System.Data;

namespace SolaERP.DataAccess.DataAcces.Implementation
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public SqlUnitOfWork(IDbConnection connection)
        {
            this._connection = connection;
            _connection.Open();
            this._transaction = _connection.BeginTransaction();
        }

        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            command.Transaction = _transaction;

            return command;
        }

        public void Dispose()
        {
            if (this._transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            if (this._connection != null)
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
