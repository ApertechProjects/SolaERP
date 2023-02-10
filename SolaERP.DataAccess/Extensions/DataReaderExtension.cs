using SolaERP.Infrastructure.Attributes;
using SolaERP.Infrastructure.Entities;
using System.Data;
using System.Reflection;

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

        /// <summary>
        ///This code implements a method that reads data from an IDataReader and returns an object of type T, where T is a subclass of BaseEntity.
        ///The method uses reflection to access the properties of the T type, and it checks each property to see if it has the DbIgnoreAttribute attribute.
        ///If it does, the property is skipped. If the property is a subclass of BaseEntity, the method invokes itself recursively to get the values for the properties of the sub-entity. 
        ///Finally, the values from the IDataReader are set to the properties of the returned object.
        ///</summary>
        /// <typeparam name="T">The type of the entity object to return.</typeparam>
        /// <param name="reader">The IDataReader to convert.</param>
        /// <returns>An entity object of type T.</returns>
        public static T GetByEntityStructure<T>(this IDataReader reader) where T : BaseEntity, new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var item in properties)
            {
                // Check if the property has a DbIgnoreAttribute attribute, if so, skip it
                if (item.GetCustomAttributes(typeof(DbIgnoreAttribute), true).Any())
                    continue;

                string propName = item.Name;
                object objectResult = null;

                // Check if the property has a DbColumnAttribute attribute, if so, Get DbColumn name from it
                var columnName = item.GetCustomAttributes(typeof(DbColumnAttribute), true)[0];
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
                        objectResult = methodInfo.MakeGenericMethod(item.PropertyType).Invoke(null, new object[] { reader });
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
        }

    }
}
