using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Dtos.Support;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Item_Code;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Entities.UOM;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace SolaERP.DataAccess.DataAccess.SqlServer
{
	public class SqlSupportRepository : ISupportRepository
	{
		private readonly IUnitOfWork _unitOfWork;
		public SqlSupportRepository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

		public async Task<int> Save(SupportSaveDto dto)
		{
			using (var command = _unitOfWork.CreateCommand() as DbCommand)
			{
				command.CommandText = @"SET NOCOUNT OFF DECLARE @Id INT 
										EXEC SP_SupportSave @Subject, @Description, @UserId,
                                        @Id = @Id OUTPUT
                                                            
                                        SELECT @Id as N'@Id'
					";

				command.Parameters.AddWithValue(command, "@Subject", dto.subject);
				command.Parameters.AddWithValue(command, "@Description", dto.description);
				command.Parameters.AddWithValue(command, "@UserId", dto.UserId);

				int newId = 0;

				using var reader = await command.ExecuteReaderAsync();

				if (reader.Read())
					newId = reader.Get<int>("@Id");

				return newId;
			}
		}

	}
}