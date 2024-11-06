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
		public DatabaseInitializerService(IConfiguration configuration
			)
		{
			_connectionString = configuration.GetConnectionString("DevelopmentConnectionString");
		}

		public void CreateOrAlterStoredProcedure()
		{
			//string procedureSql = @"
   //         CREATE OR ALTER PROCEDURE dbo.YourStoredProcedure
   //         AS
   //         BEGIN
   //             SELECT 'Hello, world!';
   //         END;
			//";
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
