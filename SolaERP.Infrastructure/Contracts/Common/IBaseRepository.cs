using SolaERP.Application.Entities;
using SolaERP.Application.Models;
using System.Data;

namespace SolaERP.Application.Contracts.Common
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> ExecQueryWithReplace(string sqlElement, List<ReplaceParams> paramListsR, List<ReplaceParams> paramListsC);
        Task<List<Parameter>> GetSqlElementParamaters(string elementName);
    }
}
