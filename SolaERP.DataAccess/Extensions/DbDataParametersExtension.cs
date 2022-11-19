using System.Data;

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
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

    }
}
