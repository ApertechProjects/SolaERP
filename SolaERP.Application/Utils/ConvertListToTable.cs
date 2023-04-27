using System.Data;
using System.Reflection;

namespace SolaERP.Persistence.Utils
{
    public static class ConvertListToTable
    {
        public static DataTable ConvertToDataTable<T>(this List<T> list) where T : class
        {
            var properties = typeof(T).GetProperties();
            DataTable table = new DataTable("MyTable");
            foreach (PropertyInfo propertyInfo in properties)
            {
                table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }

            foreach (var person in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
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
