using System.Data;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.Factories
{
    public static class ConnectionFactory
    {
        public static IDbConnection GetDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
