using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlBaseRepository : IBaseRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlBaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string ReplaceQuery(string sqlElement, ReplaceParams paramListReplaced)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT definition FROM sys.sql_modules WHERE object_id = OBJECT_ID(@storedProcedureName)";
                command.Parameters.AddWithValue(command, "@storedProcedureName", sqlElement);
                string storedProcedureDefinition = (string)command.ExecuteScalar();

                string scriptText = GetSqlElementScript(storedProcedureDefinition, sqlElement);

                scriptText = scriptText.Replace(paramListReplaced.ParamName,
                    paramListReplaced.Value == null ? "NULL" : paramListReplaced.Value.ToString());

                return scriptText;
            }
        }

        private string GetSqlElementScript(string scriptText, string sqlElementName)
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


        public async Task<List<Parameter>> GetSqlElementParamatersAsync(string elementName)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $@"select  
                                       'Parameter_name' = name,  
                                       'Type' = type_name(user_type_id)  
	                                    from sys.parameters where object_id = object_id('{elementName}')";

                List<Parameter> parameters = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    parameters.Add(reader.GetByEntityStructure<Parameter>());

                return parameters;
            }
        }

        private async Task<string> GetElementTypeAsync(string elementName)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $@"SELECT 
                                       ROUTINE_TYPE
                                       FROM 
                                        information_schema.routines 
                                        WHERE routine_name = '{elementName}';";

                string elemenType = string.Empty;
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    elemenType = reader.Get<string>("ROUTINE_TYPE");

                return elemenType;
            }
        }
    }
}