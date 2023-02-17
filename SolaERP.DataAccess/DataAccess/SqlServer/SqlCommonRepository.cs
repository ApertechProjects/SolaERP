using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlCommonRepository : ICommonRepository
    {
        //public Task<bool> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramLists)
        //{
        //    //using System.Data.SqlClient;

        //    //// ...

        //    //string newWord = "newWord";
        //    //string connectionString = "YourConnectionString";
        //    //string storedProcedureName = "dbo.SP_RequestMain_IUD";

        //    //using (SqlConnection connection = new SqlConnection(connectionString))
        //    //{
        //    //    connection.Open();
        //    //    using (SqlCommand command = new SqlCommand("SELECT definition FROM sys.sql_modules WHERE object_id = OBJECT_ID(@storedProcedureName)", connection))
        //    //    {
        //    //        command.Parameters.AddWithValue("@storedProcedureName", storedProcedureName);
        //    //        string storedProcedureDefinition = (string)command.ExecuteScalar();

        //    //        string updatedStoredProcedureDefinition = storedProcedureDefinition.Replace("APT", newWord);

        //    //        using (SqlCommand updateCommand = new SqlCommand(updatedStoredProcedureDefinition, connection))
        //    //        {
        //    //            updateCommand.ExecuteNonQuery();
        //    //        }
        //    //    }
        //    //}



        //}
    }
}
