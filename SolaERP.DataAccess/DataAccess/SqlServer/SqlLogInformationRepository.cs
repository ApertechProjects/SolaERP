using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.LogInfo;
using SolaERP.Infrastructure.Enums;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlLogInformationRepository : ILogInformationRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlLogInformationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LogInfo>> GetAllLogInformationAsync(int id, LogType logType)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "EXEC SP_LogsLoad @Id,@LogTypeId";

                command.Parameters.AddWithValue(command, "@Id", id);
                command.Parameters.AddWithValue(command, "@LogTypeId", Convert.ToInt16(logType));

                using var reader = await command.ExecuteReaderAsync();

                List<LogInfo> logInformations = new List<LogInfo>();
                while (reader.Read())
                {
                    logInformations.Add(GetLogInformationFromReader(reader));
                }

                return logInformations;
            }
        }

        private LogInfo GetLogInformationFromReader(IDataReader reader)
        {
            return new()
            {
                LogId = reader.Get<int>("LogId"),
                ActionId = reader.Get<int>("ActionId"),
                ActionName = reader.Get<string>("ActionName"),
                BusnessUnitName = reader.Get<string>("BusinessUnitName"),
                FullName = reader.Get<string>("FullName"),
                Date = reader.Get<DateTime>("Date"),
                Reference = reader.Get<string>("Reference"),
                SourceId = reader.Get<int>("SourceId"),
                LogInformation = reader.Get<string>("LogInformation")
            };
        }
    }
}
