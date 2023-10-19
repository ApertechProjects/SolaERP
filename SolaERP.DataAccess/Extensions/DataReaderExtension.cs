using Newtonsoft.Json.Linq;
using SolaERP.Application.Attributes;
using SolaERP.Application.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace SolaERP.DataAccess.Extensions
{
    public static class DataReaderExtension
    {

        public static T? Get<T>(this IDataReader reader, string columnName)
        {
            T returnType = default(T);
            var value = reader[columnName];

            if (value != DBNull.Value && value != null)
                returnType = (T)value;
            return returnType;
        }

        public static System.DateTime? GetDate<DateTime>(this IDataReader reader, string columnName)
        {
            string value1 = string.IsNullOrEmpty(reader[columnName]?.ToString()) ? null : reader[columnName].ToString();

            if (Convert.ToDateTime(value1) == System.DateTime.MinValue)
            {
                return null;
            }
            return Convert.ToDateTime(value1);
        }

        public static T GetByEntityStructure<T>(this IDataReader reader, params string[] ignoredProperties) where T : BaseEntity, new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var item in properties)
            {
                // Check if the property has a DbIgnoreAttribute attribute, if so, skip it
                if (item.GetCustomAttributes(typeof(DbIgnoreAttribute), true).Any())
                    continue;

                if (ignoredProperties.Contains(item.Name)) continue;

                string propName = item.Name;
                object objectResult = null;

                // Check if the property has a DbColumnAttribute attribute, if so, Get DbColumn name from it
                var columnName = item.GetCustomAttribute(typeof(DbColumnAttribute), true);
                if (columnName is not null)
                {
                    DbColumnAttribute columnActualName = columnName as DbColumnAttribute;
                    if (columnActualName != null) { propName = columnActualName.ColumnName; }
                }

                // Check if the property type is a subclass of BaseEntity
                if (item.PropertyType.IsSubclassOf(typeof(BaseEntity)))
                {
                    var methodInfo = typeof(DataReaderExtension).GetMethod("GetByEntityStructure", BindingFlags.Static | BindingFlags.Public);
                    // If the method was found, invoke it and store the result
                    if (methodInfo != null)
                    {
                        objectResult = methodInfo.MakeGenericMethod(item.PropertyType).Invoke(null, new object[] { reader, ignoredProperties });
                        CheckObjectResultAndSetToEntity(item, obj, objectResult);
                    }
                }
                else
                {
                    // If the property type is not a subclass of BaseEntity, get the value from the reader
                    objectResult = reader[propName];
                    CheckObjectResultAndSetToEntity(item, obj, objectResult);
                }
            }
            return obj;
        }

        private static void CheckObjectResultAndSetToEntity(PropertyInfo property, object entity, object objectResult)
        {
            if (objectResult != DBNull.Value && objectResult != null)
                property.SetValue(entity, objectResult);
            else
                property.SetValue(entity, null);
        }

        private static string GenerateEntityMappingScript<T>(IDataReader reader)
        {
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine($"private {typeof(T).Name} Get{typeof(T).Name}FromReader(IDataReader reader)")
                        .AppendLine("{")
                        .AppendLine($"\t{typeof(T).Name} entity = new {typeof(T).Name}();")
                        .AppendLine();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string columnName = property.Name;
                string propertyType = property.PropertyType.Name;
                string getValueMethod = GetReaderGetMethod(propertyType);

                scriptBuilder.AppendLine($"\tentity.{property.Name} = reader.{getValueMethod}(\"{columnName}\");");
            }

            scriptBuilder.AppendLine()
                        .AppendLine("\treturn entity;")
                        .AppendLine("}");

            return scriptBuilder.ToString();
        }

        private static string GetReaderGetMethod(string propertyType)
        {
            switch (propertyType)
            {
                case "String":
                    return "GetString";
                case "Int32":
                    return "GetInt32";
                case "Decimal":
                    return "GetDecimal";
                case "DateTime":
                    return "GetDateTime";
                case "Boolean":
                    return "GetBoolean";
                // Add more cases for other property types if needed
                default:
                    throw new ArgumentException($"Unsupported property type: {propertyType}");
            }
        }

        public static List<T> GetDataByFilter<T>(this List<T> list, string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return list;

            List<T> values = new List<T>();
            filter = filter.ToLower();
            var properties = typeof(T).GetProperties();
            foreach (var item in list)
            {
                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType == typeof(String) && property.GetValue(item, null).ToString().CheckNullAndApplyLower().Contains(filter))
                    {
                        values.Add(item);
                    }
                }
            }
            return values;
        }

        public static string CheckNullAndApplyLower(this string value)
        {
            value = value ?? "";
            return value.ToLower();
        }


    }
}
