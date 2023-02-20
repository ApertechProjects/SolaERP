using AutoMapper;
using SolaERP.Infrastructure.Contracts.Common;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities;
using SolaERP.Infrastructure.Entities.Item_Code;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Application.Services
{
    public class CommonService<TEntity, TEntityDto> : ICommonService<TEntityDto> where TEntity : BaseEntity
    {
        private ICommonRepository<TEntity> _commonRepository;
        private IMapper _mapper;
        public CommonService(ICommonRepository<TEntity> commonRepository, IMapper mapper)
        {
            _commonRepository = commonRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<TEntityDto>>> ExecQueryWithReplace(string sqlElement, List<ExecuteQueryParamList> paramListsR, List<ExecuteQueryParamList> paramListsC)
        {
            var data = await _commonRepository.ExecQueryWithReplace(sqlElement, paramListsR, paramListsC);
            var dto = _mapper.Map<List<TEntityDto>>(data);
            return ApiResponse<List<TEntityDto>>.Success(dto, 200);
        }
    }
}
