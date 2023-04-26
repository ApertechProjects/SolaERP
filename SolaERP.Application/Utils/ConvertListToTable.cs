using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Utils
{
    public static class ConvertListToTable
    {
        public static DataTable ConvertListToDataTable<T>(this List<T> list) where T : ICollection<T>
        {
            PropertyInfo[] propertyInfos = null;
            propertyInfos = list.GetType().GetProperties();

            PropertyInfo item = propertyInfos[2];
            for (int i = 0; i < list.Count; i++)
            {
                var prop = item.GetValue(list);

            }



            if (prop is IEnumerable)
            {
                foreach (var listitem in prop as IEnumerable)
                {
                }
            }
            return new DataTable();
        }
    }
}
