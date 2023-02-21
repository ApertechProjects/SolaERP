using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Models;
using System.Data;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> ExecQueryWithReplace(string sqlElement, List<ReplaceParams> paramListsR, List<ReplaceParams> paramListsC);
    }
}
