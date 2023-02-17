using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Entities.Item_Code;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data.Common;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlCommonRepository : ICommonRepository<ItemCodeWithImages>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlCommonRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ItemCodeWithImages>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListR, List<ExecuteQueryParamList> paramListC)
        {
            #region
            //string newWord = "newWord";
            //string connectionString = "YourConnectionString";
            //string storedProcedureName = "dbo.SP_RequestMain_IUD";

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    using (SqlCommand command = new SqlCommand("SELECT definition FROM sys.sql_modules WHERE object_id = OBJECT_ID(@storedProcedureName)", connection))
            //    {
            //        command.Parameters.AddWithValue("@storedProcedureName", storedProcedureName);
            //        string storedProcedureDefinition = (string)command.ExecuteScalar();

            //        string updatedStoredProcedureDefinition = storedProcedureDefinition.Replace("APT", newWord);

            //        using (SqlCommand updateCommand = new SqlCommand(updatedStoredProcedureDefinition, connection))
            //        {
            //            updateCommand.ExecuteNonQuery();
            //        }
            //    }
            //}
            #endregion
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT definition FROM sys.sql_modules WHERE object_id = OBJECT_ID(@storedProcedureName)";
                command.Parameters.AddWithValue(command, "@storedProcedureName", sqlElement);
                string storedProcedureDefinition = (string)command.ExecuteScalar();

                string scriptText = GetSqlElementScript(storedProcedureDefinition, sqlElement);
                string updatedStoredProcedureDefinition = scriptText;

                for (int i = 0; i < paramListR.Count; i++)
                    updatedStoredProcedureDefinition = scriptText.Replace(paramListR[i].ParamName,
                        paramListR[i].Value == null ? "NULL" : paramListR[i].Value.ToString());

                using (SqlCommand updateCommand = new SqlCommand(updatedStoredProcedureDefinition))
                {
                    for (int i = 0; i < paramListC.Count; i++)
                        updateCommand.Parameters.AddWithValue(updateCommand, paramListC[i].ParamName, paramListC[i].Value);

                    ItemCodeWithImages result = new();
                    using var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result = reader.GetByEntityStructure<ItemCodeWithImages>();
                    }
                }
            }
            return new();
        }

        public static string GetSqlElementScript(string scriptText, string sqlElementName)
        {
            if (sqlElementName.StartsWith("[dbo].[FN_") || sqlElementName.StartsWith("[dbo].[GET_"))
                scriptText = scriptText.Substring(scriptText.IndexOf("SELECT"), scriptText.LastIndexOf(")") - scriptText.IndexOf("SELECT"));
            else if (sqlElementName.StartsWith("[dbo].[VW_"))
                scriptText = scriptText.Substring(scriptText.IndexOf("SELECT"));
            else if (sqlElementName.StartsWith("[dbo].[SP_"))
                scriptText = scriptText.Substring(scriptText.IndexOf("BEGIN") + 5, scriptText.LastIndexOf("END") - scriptText.IndexOf("BEGIN") - 5);
            else if (sqlElementName.StartsWith("[dbo].[SF_"))
                scriptText = "SELECT" + scriptText.Substring(scriptText.IndexOf(" = ") + 2, scriptText.LastIndexOf("RETURN") - scriptText.IndexOf(" = ") - 2);

            return scriptText;
        }
    }
}