using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Entities.AccountCode;
using SolaERP.Application.Entities.GridLayout;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlGridLayoutRepository : SqlBaseRepository, IGridLayoutRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlGridLayoutRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(GridLayout entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"INSERT INTO [dbo].[GridLayouts]
                                               ([UserId]
                                               ,[LayoutName]
                                               ,[LayoutData])
                                         VALUES
                                               (@UserId
                                               ,@LayoutName
                                               ,@LayoutData)";

                command.Parameters.AddWithValue(command, "@UserId", entity.UserId);
                command.Parameters.AddWithValue(command, "@LayoutName", entity.LayoutName);
                command.Parameters.AddWithValue(command, "@LayoutData", entity.LayoutData);

                await command.ExecuteNonQueryAsync();
            }
                
            return true;
        }

        public async Task<List<GridLayout>> GetAllAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SELECT [Id]
                                          ,[UserId]
                                          ,[LayoutName]
                                          ,[LayoutData]
                                      FROM [dbo].[GridLayouts]";
                using var reader = await command.ExecuteReaderAsync();

                List<GridLayout> accountCodes = new List<GridLayout>();

                while (reader.Read())
                {
                    accountCodes.Add(reader.GetByEntityStructure<GridLayout>());
                }
                return accountCodes;
            }
        }

        public async Task<GridLayout> GetAsync(int userId, string layoutName)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SELECT [Id]
                                          ,[UserId]
                                          ,[LayoutName]
                                          ,[LayoutData]
                                      FROM [dbo].[GridLayouts]
                                      where UserId = @UserId and LayoutName = @LayoutName";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@LayoutName", layoutName);

                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    return reader.GetByEntityStructure<GridLayout>();
                }
                return null;
            }
        }

        public async Task<GridLayout> GetByIdAsync(int id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"SELECT [Id]
                                          ,[UserId]
                                          ,[LayoutName]
                                          ,[LayoutData]
                                      FROM [dbo].[GridLayouts]
                                      where Id = @Id";
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    return reader.GetByEntityStructure<GridLayout>();
                }
                return null;
            }
        }

        public async Task<bool> RemoveAsync(int Id)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"DELETE FROM [dbo].[GridLayouts] where Id = @Id";

                command.Parameters.AddWithValue(command, "@Id", Id);

                await command.ExecuteNonQueryAsync();
            }

            return true;
        }

        public async Task UpdateAsync(GridLayout entity)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @"UPDATE [dbo].[GridLayouts]
                                       SET [UserId] = @UserId
                                          ,[LayoutName] = @LayoutName
                                          ,[LayoutData] = @LayoutData
                                     WHERE Id = @Id";

                command.Parameters.AddWithValue(command, "@Id", entity.Id);
                command.Parameters.AddWithValue(command, "@UserId", entity.UserId);
                command.Parameters.AddWithValue(command, "@LayoutName", entity.LayoutName);
                command.Parameters.AddWithValue(command, "@LayoutData", entity.LayoutData);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
