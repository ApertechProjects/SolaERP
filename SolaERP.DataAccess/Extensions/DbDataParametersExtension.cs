using System.Data;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.Extensions
{
    public static class DbDataParametersExtension
    {
        public static IDbDataParameter AddWithValue(this IDbDataParameter parameter, string propName, object value)
        {
            parameter.ParameterName = propName;
            parameter.Value = value;

            return parameter;
        }

        public static void AddWithValue(this IDataParameterCollection parameters, IDbCommand command, string parameterName, object value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value == null ? DBNull.Value : value;
            command.Parameters.Add(parameter);
        }

        public static void AddOutPutParameter(this IDataParameterCollection parameters, IDbCommand command, string parameterName)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.Output;
            parameter.DbType = DbType.Int32;
            command.Parameters.Add(parameter);
        }

        public static void AddTableValue(this IDataParameterCollection parameters, IDbCommand command, string parameterName, string typeName, DataTable dataTable)
        {
            SqlParameter parameter = command.CreateParameter() as SqlParameter;
            parameter.ParameterName = parameterName;
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.Value = dataTable;
            parameter.TypeName = typeName;

            parameters.Add(parameter);  

        }
    }
}
