using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface ICommonRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListsR, List<ExecuteQueryParamList> paramListsC);
    }
}
