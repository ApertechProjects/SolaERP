using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Entities.Vendors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolaERP.Application.Helper
{
    public static class Compare
    {
        public static List<string> CompareRow<T>(T oldVersion, T newVersion) where T : class, new()

        {
            if (oldVersion == null || newVersion == null)
            {
                throw new ArgumentNullException("Objects cannot be null.");
            }

            List<string> differences = new();

            Type type = typeof(T);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                object oldValue = property.GetValue(oldVersion);
                object newValue = property.GetValue(newVersion);

                if (oldValue == null && newValue == null)
                {
                    continue;
                }
                if (oldValue != newValue)
                {
                    string propertyName = property.Name;

                    if (propertyName.Contains("Status") ||
                        propertyName == "VendorId")
                        continue;

                    if (propertyName.Contains("_"))
                        propertyName = propertyName.Replace("_", "/");

                    if (property.Name.EndsWith("Id"))
                        propertyName = TrimEndWithWord(propertyName, "Id");

                    differences.Add(SeparateWords(propertyName));
                }
            }

            return differences;
        }

        private static string SeparateWords(string input)
        {
            // Regular expression to add a space before each uppercase letter (except the first one)
            return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
        }

        private static string TrimEndWithWord(string input, string cut)
        {
            ReadOnlySpan<char> inputSpan = input.AsSpan();
            ReadOnlySpan<char> cutSpan = cut.AsSpan();

            // Check if the input ends with the word to be trimmed
            if (inputSpan.EndsWith(cutSpan))
            {
                // Trim the word by slicing the span and trimming any remaining spaces
                return inputSpan.Slice(0, inputSpan.Length - cutSpan.Length).ToString().TrimEnd();
            }

            // If the word is not at the end, return the original string
            return input;
        }
    }
}
