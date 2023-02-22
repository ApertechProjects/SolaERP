using SolaERP.Console.Extensions;
using System.Data;

public class Program
{
    private static void Main(string[] args)
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("HeadPersonId", typeof(string));
        dataTable.Columns.Add("HeadPersonName1", typeof(string));
        dataTable.Columns.Add("HeadPersonName2", typeof(string));
        dataTable.Columns.Add("HeadPersonName3", typeof(string));
        dataTable.Columns.Add("HeadPersonName4", typeof(string));
        dataTable.Columns.Add("HeadPersonName5", typeof(string));
        dataTable.Columns.Add("HeadPersonName6", typeof(string));
        dataTable.Columns.Add("HeadPersonName7", typeof(string));
        dataTable.Columns.Add("HeadPersonName8", typeof(string));
        dataTable.Columns.Add("HeadPersonName9", typeof(string));
        dataTable.Columns.Add("HeadPersonName00", typeof(string));
        dataTable.Columns.Add("HeadPersonName000", typeof(string));
        dataTable.Rows.Add("123", "John");

        dataTable.GenerateClassFromDataTable("Test", @"C:\Users\HP\Desktop\New folder");
    }
}