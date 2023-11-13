using SolaERP.Persistence.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Utils
{
    public static class ConvertBusinessUnit
    {
        public static string GetBusinessUnitCode(this int businessUnitId)
        {
            BusinessCode businessCode = (BusinessCode)businessUnitId;
            string result = businessCode.ToString();
            return result;
        }
    }
}
