using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Utils
{
    public static class StringHelper
    {
        public static string CheckNullAndApplyLower(this string value)
        {
            value = value ?? "";
            return value.ToLower();
        }
    }
}
