using SolaERP.Application.Contracts.Common;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.AccountCode;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Enums;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlQueryBuilder : IQueryBuilder
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlQueryBuilder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Type ConvertToCSharpType(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "bigint":
                    return typeof(long);
                case "binary":
                case "image":
                case "varbinary":
                    return typeof(byte[]);
                case "bit":
                    return typeof(bool);
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                    return typeof(string);
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return typeof(DateTime);
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    return typeof(decimal);
                case "float":
                    return typeof(double);
                case "int":
                    return typeof(int);
                case "real":
                    return typeof(float);
                case "smallint":
                    return typeof(short);
                case "time":
                    return typeof(TimeSpan);
                case "tinyint":
                    return typeof(byte);
                case "uniqueidentifier":
                    return typeof(Guid);
                default:
                    throw new ArgumentException($"Unrecognized SQL type: {sqlType}");
            }
        }
        private string ConvertToCSharpTypeAsString(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "bigint":
                    return "long";
                case "binary":
                case "image":
                case "varbinary":
                    return "byte[]";
                case "bit":
                    return "bool";
                case var type when
                type.StartsWith("char") ||
                type.StartsWith("nchar") ||
                type.StartsWith("ntext") ||
                type.StartsWith("nvarchar") ||
                type.StartsWith("varchar"):
                    return "string";
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return "DateTime";
                case var type when
                type.StartsWith("decimal"):
                case "money":
                case "numeric":
                case "smallmoney":
                    return "decimal";
                case "float":
                    return "double";
                case "int":
                    return "int";
                case "real":
                    return "float";
                case "smallint":
                    return "short";
                case "time":
                    return "TimeSpan";
                case "tinyint":
                    return "byte";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    return "object";
            }
        }



        public string GenerateSqlQuery(SqlElementTypes type, string elementName, List<Parameter> elementParamters)
        {
            string generatedQuery = type switch
            {
                SqlElementTypes.PROCEDURE => $@"EXEC {elementName} ",
                SqlElementTypes.FUNCTION => $@"SELECT * FROM {elementName}("
            };

            StringBuilder queryBuilder = new StringBuilder(generatedQuery);
            foreach (Parameter item in elementParamters)
                queryBuilder.AppendLine(item.ParameterName + ",");

            queryBuilder.Length--;

            if (type.Equals(SqlElementTypes.FUNCTION))
                queryBuilder.Append(")");

            string sqlQuery = queryBuilder.ToString();
            sqlQuery = sqlQuery.Remove(sqlQuery.LastIndexOf(","));

            return sqlQuery;
        }

        public async Task<string> GenerateCLRQueryAsync(string elementName)
        {
            List<Parameter> elelementParameters = await GetSqlElementParamatersAsync(elementName);
            SqlElementTypes elementType = await GetElementTypeAsync(elementName);

            string generatedSqlQuery = GenerateSqlQuery(elementType, elementName, elelementParameters);

            StringBuilder clrQueryBuilder = new();
            clrQueryBuilder.AppendLine($"using (var command = _unitOfWork.CreateCommand() as DbCommand)")
                   .AppendLine("{")
                   .AppendLine($"\tcommand.CommandText = \"{generatedSqlQuery}\";")
                   .AppendLine();


            foreach (Parameter parameter in elelementParameters)
                clrQueryBuilder.AppendLine($"\tcommand.Parameters.AddWithValue(command, \"{parameter.ParameterName}\",);\n");


            clrQueryBuilder.AppendLine("\treturn await command.ExecuteNonQueryAsync() > 0;")
                   .AppendLine("}");


            return clrQueryBuilder.ToString();
        }


        public List<Parameter> GetSqlElementParameters(string sqlElementName, CommandType type)
        {
            List<Parameter> parameters = new List<Parameter>();

            using (SqlCommand command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandType = type;
                command.CommandText = sqlElementName;
                SqlCommandBuilder.DeriveParameters(command);

                foreach (SqlParameter parameter in command.Parameters)
                {
                    if (parameter.Direction != ParameterDirection.ReturnValue)
                    {
                        parameters.Add(new Parameter
                        {
                            ParameterName = parameter.ParameterName,
                            Type = parameter.SqlDbType.ToString()
                        });
                    }
                }

                return parameters;
            }
        }

        public string GenerateClassFieldsFromSqlElement(string className, string sqlElementName, CommandType type)
        {
            List<Parameter> elementParameters = GetSqlElementParameters(sqlElementName, type);
            StringBuilder classBuilder = new StringBuilder();

            classBuilder.AppendLine($"public class {className}")
                        .AppendLine("{");

            foreach (Parameter parameter in elementParameters)
            {
                string fieldName = GenerateFieldName(parameter.ParameterName);
                string fieldType = ConvertToCSharpTypeAsString(parameter.Type);

                classBuilder.AppendLine($"\tpublic {fieldType} {fieldName} {{ get; set; }}");
            }

            classBuilder.AppendLine("}");
            return classBuilder.ToString();
        }

        public string GetSqlElementResult(string className, string elementName, SqlElementTypes sqlElementTypes)
        {
            List<Parameter> elementParameters = GetSqlElementParameters(elementName, sqlElementTypes);

            StringBuilder classBuilder = new StringBuilder();

            classBuilder.AppendLine($"public class {className}")
                        .AppendLine("{");

            foreach (Parameter parameter in elementParameters)
            {
                string fieldName = GenerateFieldName(parameter.ParameterName);
                string fieldType = ConvertToCSharpTypeAsString(parameter.Type);

                classBuilder.AppendLine($"\tpublic {fieldType} {fieldName} {{ get; set; }}");
            }

            classBuilder.AppendLine("}");
            return classBuilder.ToString();
        }

        public List<Parameter> GetSqlElementParameters(string elementName, SqlElementTypes sqlElementTypes)
        {
            List<Parameter> parameters = new List<Parameter>();

            using (SqlCommand command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = GenerateCommandText(sqlElementTypes);
                command.Parameters.AddWithValue(command, "@elementName", elementName);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    parameters.Add(reader.GetByEntityStructure<Parameter>());
                }

                return parameters;
            }
        }


        public string GenerateCommandText(SqlElementTypes sqlElementTypes)
        {
            string text = string.Empty;
            switch (sqlElementTypes)
            {
                case SqlElementTypes.PROCEDURE:
                    text = @"SELECT 
                             name AS Parameter_name,
                             system_type_name AS Type
                             FROM sys.dm_exec_describe_first_result_set
                             (N'EXEC ' + @elementName, NULL, 0);";
                    break;
                case SqlElementTypes.FUNCTION:
                    text = @"SELECT 
                             c.name AS Parameter_name,
                             t.name AS Type
                             FROM sys.columns c
                             JOIN sys.objects o ON c.object_id = o.object_id
                             JOIN sys.types t ON c.user_type_id = t.user_type_id
                             WHERE o.name = @elementName";
                    break;
                default:
                    break;
            }
            return text;
        }

        private string GenerateFieldName(string parameterName)
        {
            string fieldName = parameterName.TrimStart('@');
            if (fieldName.Length > 0)
            {
                fieldName = fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
            }

            return fieldName;
        }

        public async Task<List<Parameter>> GetSqlElementParamatersAsync(string elementName)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $@"select  
                                       'Parameter_name' = name,  
                                       'Type' = type_name(user_type_id)  
	                                    from sys.parameters where object_id = object_id(@elementName)";


                command.Parameters.AddWithValue(command, "@elementName", elementName);

                List<Parameter> parameters = new();
                using var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                    parameters.Add(reader.GetByEntityStructure<Parameter>());

                return parameters;
            }
        }

        public async Task<SqlElementTypes> GetElementTypeAsync(string elementName)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = $@"SELECT 
                                       ROUTINE_TYPE
                                       FROM 
                                        information_schema.routines 
                                        WHERE routine_name = @elementName;";

                command.Parameters.AddWithValue(command, "@elementName", elementName);

                string elemenType = string.Empty;
                using var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                    elemenType = reader.Get<string>("ROUTINE_TYPE");

                return elemenType.Equals("PROCEDURE") ? SqlElementTypes.PROCEDURE : SqlElementTypes.FUNCTION;
            }
        }



    }
}
