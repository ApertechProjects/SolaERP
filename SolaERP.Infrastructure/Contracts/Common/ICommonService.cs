using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface ICommonService<T>
    {
        Task<ApiResponse<List<T>>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListsR, List<ExecuteQueryParamList> paramListsC);
    }
}
