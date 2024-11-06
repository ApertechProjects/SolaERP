using Microsoft.Extensions.Configuration;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.DataAcces.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.Helper
{
	public class DatabaseInitializerService
	{
		private readonly string _connectionString;
		private readonly IUnitOfWork _unitOfWork;
		public DatabaseInitializerService(IConfiguration configuration
			//, IUnitOfWork unitOfWork
			)
		{
			_connectionString = configuration.GetConnectionString("DevelopmentConnectionString");
			//_unitOfWork = new SqlUnitOfWork(new );
		}

		public void CreateOrAlterStoredProcedure()
		{
			//string procedureSql = @"
   //         IF OBJECT_ID('dbo.YourStoredProcedure', 'P') IS NOT NULL
   //         BEGIN
   //             DROP PROCEDURE dbo.YourStoredProcedure;
   //         END;

   //         CREATE PROCEDURE dbo.YourStoredProcedure
   //         AS
   //         BEGIN
   //             SELECT 'Hello, world!';
   //         END;
   //     ";
			//using (SqlConnection connection = new SqlConnection(_connectionString))
			//{
			//	connection.Open();
			//	using (SqlCommand command = new SqlCommand(procedureSql, connection))
			//	{
			//		command.ExecuteNonQuery();
			//	}
			//}

		}
	}
}
