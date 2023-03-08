

using SolaERP.Business.Models;

namespace SolaERP.Business.CommonLogic
{
    public static class Utility
    {
        /// <summary>
        ///    TableNameWithSchema param format = "SchemaName.TableName" 
        /// </summary>
        /// <param name="TableNameWithSchema"></param>
        /// <param name="confHelper"></param>
        /// <returns></returns>
        public static async Task<int> GetMaxId(string tableNameWithSchema, ConfHelper confHelper)
        {
            var maxId = Convert.ToInt32((await GetData.FromQuery($"EXEC SP_GET_MAX_ID '{tableNameWithSchema.Split('.')[1]}','{tableNameWithSchema.Split('.')[0]}'", confHelper.DevelopmentUrl)).Rows[0][0]);

            return maxId;
        }
    }
}



