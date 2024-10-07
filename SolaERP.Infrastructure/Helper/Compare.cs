using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Entities.Vendors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Helper
{
    public static class Compare
    {
        public static T CompareRow<T>(T oldVersion, T newVersion) where T : class, new()

        {
            if (oldVersion == null || newVersion == null)
            {
                throw new ArgumentNullException("Objects cannot be null.");
            }

            T differences = new T(); // Create a new instance of T

            // Get the type of the generic class T
            Type type = typeof(T);

            // Get all properties of the generic class T
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // Get the old and new values for each property
                object oldValue = property.GetValue(oldVersion);
                object newValue = property.GetValue(newVersion);

                // Compare values (consider nulls)
                if (oldValue == null && newValue == null)
                {
                    continue; // Skip if both are null
                }
                if (!oldValue.Equals(newValue))
                {
                    // If there is a difference, set the new value in the differences object
                    property.SetValue(differences, newValue);
                }
            }

            return differences;
        }

        private static void CheckObjectResultAndSetToEntity(PropertyInfo property, object entity, object objectResult)
        {
            if (objectResult != DBNull.Value && objectResult != null)
                property.SetValue(entity, objectResult);
            else
                property.SetValue(entity, null);
        }
    }
}
