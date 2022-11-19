using SolaERP.Infrastructure.Attributes;
using SolaERP.Infrastructure.Entities;
using System.Data;

namespace SolaERP.DataAccess.Extensions
{
    public static class DataReaderExtension
    {
        public static T Get<T>(this IDataReader reader, string columnName)
        {
            T returnType = default(T);
            var value = reader[columnName];

            if (value != DBNull.Value && value != null)
                returnType = (T)value;

            return returnType;
        }

        public static T GetByEntityStructure<T>(this IDataReader reader) where T : BaseEntity, new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            foreach (var item in properties)
            {
                var itemAttribute = item.GetCustomAttributes(typeof(DbIgnoreAttribute), true);
                var dbIgnore = itemAttribute.Select(x => x.GetType() == typeof(DbIgnoreAttribute)).Count();
                string propName = item.Name;

                if (dbIgnore == 0)
                {
                    var value = reader[propName];
                    if (value != DBNull.Value && value != null)
                        item.SetValue(obj, value);
                }
            }
            return obj;
        }

    }
}
