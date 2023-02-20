using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Item_Code;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Services
{
    public class CommonService : ICommonService<ItemCode>
    {
        private ICommonRepository<ItemCode> _commonRepository;
        public CommonService(ICommonRepository<ItemCode> commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task<ApiResponse<List<ItemCode>>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListsR, List<ExecuteQueryParamList> paramListsC)
        {
            var ttt = await _commonRepository.ExecQueryWithReplace(sqlElement, paramListsR, paramListsC);
            return ApiResponse<List<ItemCode>>.Success(ttt, 200);
        }
    }
}
