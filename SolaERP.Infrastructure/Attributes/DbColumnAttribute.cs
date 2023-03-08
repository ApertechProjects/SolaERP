namespace SolaERP.Infrastructure.Attributes
{
    public class DbColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public DbColumnAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }
    }
}
