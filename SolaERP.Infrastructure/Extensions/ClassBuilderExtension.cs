using System.Data;

namespace SolaERP.Application.Extensions
{
    public static class ClassBuilderExtension
    {
        public static string GetDataTableColumNames(this DataTable dataTable, string contentRootPath)
        {
            string columNames = string.Empty;
            foreach (DataColumn columnName in dataTable.Columns)
            {
                columNames += $"public {columnName.DataType} {columnName.ColumnName.Replace(" ", "").Replace("/", "")}" + " " + "{ get; set; }" + Environment.NewLine;
            }

            if (File.Exists(Path.Combine(contentRootPath, "ColNames.txt")))
                File.Delete(Path.Combine(contentRootPath, "ColNames.txt"));

            using (StreamWriter sw = new StreamWriter(Path.Combine(contentRootPath, "ColNames.txt"), true))
            {
                sw.WriteLine(columNames);
                sw.Close();
                sw.Dispose();
            }
            return columNames;
        }


    }
}
