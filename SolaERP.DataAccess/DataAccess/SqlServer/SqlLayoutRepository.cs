using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Layout;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlLayoutRepository : ILayoutRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlLayoutRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteLayoutAsync(int userId, string layoutKey)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_SaveLayout_D @UserId,@Key";

                command.Parameters.AddWithValue(command, "@UserId", userId);
                command.Parameters.AddWithValue(command, "@Key", layoutKey);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<Layout> GetUserLayoutAsync(int userId, string layoutKey)
        {
            using (var commmand = _unitOfWork.CreateCommand() as DbCommand)
            {
                commmand.CommandText = "EXEC SP_SaveLayout_Load @UserId,@LayoutKey";

                commmand.Parameters.AddWithValue(commmand, "@UserId", userId);
                commmand.Parameters.AddWithValue(commmand, "@LayoutKey", layoutKey);

                using var reader = await commmand.ExecuteReaderAsync();
                Layout result = new();

                if (reader.Read())
                    result = reader.GetByEntityStructure<Layout>("Filebase64");

                return result;
            }
        }

        public async Task<bool> SaveLayoutAsync(Layout layout)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_SaveLayout_IU @UserId,@Key,@Layout,@TabIndex";

                command.Parameters.AddWithValue(command, "@UserId", layout.UserId);
                command.Parameters.AddWithValue(command, "@Key", layout.Key);
                command.Parameters.AddWithValue(command, "@Layout", layout.UserLayout);
                command.Parameters.AddWithValue(command, "@TabIndex", layout.TabIndex);

                return await command.ExecuteNonQueryAsync() > 0;
            }

        }
    }
}
