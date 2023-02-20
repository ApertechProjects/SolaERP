using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Entities.Request;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlCommonRepository<TEntity> : ICommonRepository<TEntity> where TEntity : BaseEntity, new()
        //public class SqlCommonRepository<TEntity> where TEntity : BaseEntity, new()
        //    //ICommonRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlCommonRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TEntity>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListReplaced, List<ExecuteQueryParamList> paramListCommon)
        {
            List<TEntity> result = new List<TEntity>();

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT definition FROM sys.sql_modules WHERE object_id = OBJECT_ID(@storedProcedureName)";
                command.Parameters.AddWithValue(command, "@storedProcedureName", sqlElement);
                string storedProcedureDefinition = (string)command.ExecuteScalar();

                string scriptText = GetSqlElementScript(storedProcedureDefinition, sqlElement);
                string updatedStoredProcedureDefinition = scriptText;

                for (int i = 0; i < paramListReplaced.Count; i++)
                    updatedStoredProcedureDefinition = updatedStoredProcedureDefinition.Replace(paramListReplaced[i].ParamName,
                        paramListReplaced[i].Value == null ? "NULL" : paramListReplaced[i].Value.ToString());

                using (var updateCommand = _unitOfWork.CreateCommand() as DbCommand)
                {
                    updateCommand.CommandText = updatedStoredProcedureDefinition;
                    for (int i = 0; i < paramListCommon.Count; i++)
                        updateCommand.Parameters.AddWithValue(updateCommand, paramListCommon[i].ParamName, paramListCommon[i].Value);

                    using var reader = await updateCommand.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result.Add(reader.GetByEntityStructure<TEntity>());
                    }
                }
            }
            return result;
        }

        public async Task<List<RequestMain>> ExecQueryWithReplace2(string sqlElement, List<ExecuteQueryParamList> paramListReplaced, List<ExecuteQueryParamList> paramListCommon)
        {
            List<RequestMain> result = new List<RequestMain>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SELECT definition FROM sys.sql_modules WHERE object_id = OBJECT_ID(@storedProcedureName)";
                command.Parameters.AddWithValue(command, "@storedProcedureName", sqlElement);
                string storedProcedureDefinition = (string)command.ExecuteScalar();

                string scriptText = GetSqlElementScript(storedProcedureDefinition, sqlElement);
                string updatedStoredProcedureDefinition = scriptText;

                for (int i = 0; i < paramListReplaced.Count; i++)
                    updatedStoredProcedureDefinition = updatedStoredProcedureDefinition.Replace(paramListReplaced[i].ParamName,
                        paramListReplaced[i].Value == null ? "NULL" : paramListReplaced[i].Value.ToString());

                using (var updateCommand = _unitOfWork.CreateCommand() as DbCommand)
                {
                    updateCommand.CommandText = updatedStoredProcedureDefinition;
                    for (int i = 0; i < paramListCommon.Count; i++)
                        updateCommand.Parameters.AddWithValue(updateCommand, paramListCommon[i].ParamName, paramListCommon[i].Value);

                    using var reader = await updateCommand.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        result.Add(GetWaitingForApprovalFromReader(reader));
                    }
                    return result;
                }
            }
        }

        private RequestMain GetWaitingForApprovalFromReader(IDataReader reader)
        {
            return new()
            {
                RequestMainId = reader.Get<int>("RequestMainId"),
                BusinessUnitId = reader.Get<int>("BusinessUnitId"),
                RequestTypeId = reader.Get<int>("RequestTypeId"),
                RequestNo = reader.Get<string>("RequestNo"),
                EntryDate = reader.Get<DateTime>("EntryDate"),
                RequestDate = reader.Get<DateTime>("RequestDate"),
                RequestDeadline = reader.Get<DateTime>("RequestDeadline"),
                UserId = reader.Get<int>("UserId"),
                Requester = reader.Get<int>("Requester"),
                Status = reader.Get<int>("Status"),
                SupplierCode = reader.Get<string>("SupplierCode"),
                RequestComment = reader.Get<string>("RequestComment"),
                OperatorComment = reader.Get<string>("OperatorComment"),
                QualityRequired = reader.Get<string>("QualityRequired"),
                CurrencyCode = reader.Get<string>("CurrencyCode"),
                LogisticsTotal = reader.Get<decimal>("LogisticsTotal"),
            };
        }

        //public async Task<List<TEntity>> GetDatas(TEntity entity)
        //{
        //    List<TEntity> result = new List<TEntity>();

        //    while (reader.Read())
        //    {
        //        result.Add(reader.GetByEntityStructure<TEntity>());
        //    }
        //}

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