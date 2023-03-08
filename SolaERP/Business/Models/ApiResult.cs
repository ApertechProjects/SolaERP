using System;
using System.Collections.Generic;
using System.Text;

namespace SolaERP.Business.Models
{
    public class ApiResult
    {
        public ApiResult()
        {
            ErrorList = new List<string>();
        }
        public List<string> ErrorList { get; set; }
        public bool OperationIsSuccess { get; set; }
        public bool IsAuthorized { get; set; }
        public object Data { get; set; }
    }
}
