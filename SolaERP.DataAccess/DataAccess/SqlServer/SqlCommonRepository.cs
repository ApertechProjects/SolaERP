using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    //public class SqlCommonRepository<TEntity> : ICommonRepository<TEntity> where TEntity : BaseEntity, new()
    public class SqlCommonRepository<TEntity> where TEntity : BaseEntity, new()
        //ICommonRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlCommonRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string ReplaceQuery(string sqlElement, ExecuteQueryParamList paramListReplaced)
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