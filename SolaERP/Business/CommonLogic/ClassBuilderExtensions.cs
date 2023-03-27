using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace SolaERP.Business.CommonLogic
{
    public static class ClassBuilderExtensions
    {
        public static string GetDataTableColumNames(this DataTable dataTable)
        {
            string columNames = string.Empty;
            foreach (DataColumn columnName in dataTable.Columns)
            {
                columNames += $"public {columnName.DataType} {columnName.ColumnName.Replace(" ", "").Replace("/", "").Replace("System.","")}" + " " + "{ get; set; }";
            }
            return columNames;
        }

        public static List<T> ConvertToClassListModel<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T ConvertToClassModel<T>(this DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                return GetItem<T>(dt.Rows[0]);
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (dr[column.ColumnName] != DBNull.Value)
                        {
                            pro.SetValue(obj, dr[column.ColumnName.Replace(" ", "").Replace("/", "")], null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
