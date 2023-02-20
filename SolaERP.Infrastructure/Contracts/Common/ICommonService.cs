using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Common
{
    public interface ICommonService<TEntityDto> 
    {
        Task<ApiResponse<List<TEntityDto>>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListsR, List<ExecuteQueryParamList> paramListsC);
    }
}
