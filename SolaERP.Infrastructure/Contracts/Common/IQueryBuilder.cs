using SolaERP.Application.Entities;
using SolaERP.Application.Enums;
using System.Data;

namespace SolaERP.Application.Contracts.Common
{
    public interface IQueryBuilder
    {
        Type ConvertToCSharpType(string sqlType);
        string GenerateSqlQuery(SqlElementTypes type, string elementName, List<Parameter> elementParamters);
        Task<string> GenerateCLRQueryAsync(string elementName);
        Task<List<Parameter>> GetSqlElementParamatersAsync(string elementName);
        Task<SqlElementTypes> GetElementTypeAsync(string elementName);
        List<Parameter> GetSqlElementParameters(string sqlElementName, CommandType type);
        string GenerateClassFieldsFromSqlElement(string className, string sqlElementName, CommandType type);
    }
}
