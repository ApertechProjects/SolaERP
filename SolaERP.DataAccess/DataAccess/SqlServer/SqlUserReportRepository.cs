using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.UserReport;
using SolaERP.Application.Entities.UserReport;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
	public class SqlUserReportRepository : IUserReportRepository
	{
		private readonly IUnitOfWork _unitOfWork;
		public SqlUserReportRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Delete(string reportFileId)
		{
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = "SET NOCOUNT OFF Delete from  Config.UserReportFileAccesses where ReportFileId = @ReportFileId";

				command.Parameters.AddWithValue(command, "@ReportFileId", reportFileId);

				return await command.ExecuteNonQueryAsync() > 0;
			}
		}

		public async Task<string> GetFileNameByReportFileId(int id)
		{
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = $"SELECT ReportFileName FROM [Config].[UserReportFileAccesses] where Id = {id}";
				using var reader = await command.ExecuteReaderAsync();

				string fileName = null;
				while (await reader.ReadAsync())
				{
					fileName = reader.Get<string>("ReportFileName");
				}
				return fileName;
			}
		}

		public async Task<bool> Save(UserReportFileAccess data)
		{
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = "SET NOCOUNT OFF INSERT INTO Config.UserReportFileAccesses(UserId,ReportFileId,ReportFileName) " +
		   "VALUES(@UserId,@ReportFileId,@ReportFileName) SELECT SCOPE_IDENTITY() AS NewIdentity;";

				command.Parameters.AddWithValue(command, "@UserId", data.UserId);
				command.Parameters.AddWithValue(command, "@ReportFileId", data.ReportFileId);
				command.Parameters.AddWithValue(command, "@ReportFileName", data.ReportFileName);

				return await command.ExecuteNonQueryAsync() > 0;
			}



		}

		public async Task<int> SaveTest(UserReportFileAccess data)
		{

			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				// Set the command text to insert the data and return the identity value
				command.CommandText = @"SET NOCOUNT OFF;
                            INSERT INTO Config.UserReportFileAccesses(UserId, ReportFileId, ReportFileName)
                            VALUES (@UserId, @ReportFileId, @ReportFileName);
                            SELECT SCOPE_IDENTITY() AS NewIdentity;";

				// Add parameters to the command
				command.Parameters.AddWithValue(command, "@UserId", data.UserId);
				command.Parameters.AddWithValue(command, "@ReportFileId", data.ReportFileId);
				command.Parameters.AddWithValue(command, "@ReportFileName", data.ReportFileName);

				// Execute the query and get the identity value using ExecuteScalarAsync
				var result = await command.ExecuteScalarAsync();

				// Return the new identity value as a long (cast result accordingly)
				return (int)(result != null ? Convert.ToInt64(result) : 0);
			}
		}
	}
}
