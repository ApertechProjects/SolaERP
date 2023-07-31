using SolaERP.Application.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.Extensions
{
    internal static class ListToDataTableExntension
    {
        public static DataTable ConvertToDataTable<T>(this List<T> list) where T : class
        {
            var properties = typeof(T).GetProperties();

            DataTable table = new DataTable("MyTable");
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.GetCustomAttributes(typeof(NotIncludeAttribute), true).Any())
                    continue;

                table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }

            foreach (var person in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttributes(typeof(NotIncludeAttribute), true).Any())
                        continue;

                    row[property.Name] = property.GetValue(person, null) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable ConvertListToDataTable<T>(this List<T> list)
        {
            DataTable table = new DataTable("MyTable");
            table.Columns.Add("Column", typeof(T));

            foreach (var item in list)
                table.Rows.Add(item);

            return table;
        }
    }
}
