using System.Data;
using System.Data.SqlClient;

namespace SolaERP.Business.CommonLogic
{
    public static class GetData
    {
        public static async Task<DataTable> FromQuery(string commandText, string connString)
        {
            var resultData = new DataTable();
            using (var connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = commandText;
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dr = await command.ExecuteReaderAsync();
                        resultData.Load(dr);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            return resultData;
        } 
    }
}
