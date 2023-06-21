using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Utils
{
    public static class ConvertDate
    {
        public static DateTime? ConvertDateToValidDate(this DateTime? date)
        {
            if (date?.Date.Year < 1800)
                return null;
            else
                return date;
        }
    }
}
