using SolaERP.Business.Models;
using SolaERP.Business.CommonLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SolaERP.Business.CommonLogic
{
    public class GenericMapLogic<T>
    {
        public ApiResult BuildModel(DataTable dataTable)
        {
            var result = new ApiResult();
            if (dataTable is null)
            {
                result.ErrorList.Add("Data is null.Please,Contact Administration");
                return result;
            }
            #pragma warning disable 8601
            result.Data = dataTable.Rows.Count > 1 ? dataTable.ConvertToClassListModel<T>() : dataTable.ConvertToClassModel<T>();
            result.OperationIsSuccess = true;
            result.IsAuthorized = true;
            return result;
        }
    }
}
