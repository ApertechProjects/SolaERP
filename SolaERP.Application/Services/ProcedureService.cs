using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Procedure;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;
        private IMapper _mapper;

        public ProcedureService(IProcedureRepository procedureRepository, IMapper mapper)
        {
            _procedureRepository = procedureRepository;
            _mapper = mapper;
        }
        public Task AddAsync(ProcedureDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<ProcedureDto>>> GetAllAsync()
        {
            var procedureDto = await _procedureRepository.GetAllAsync();
            var dto = _mapper.Map<List<ProcedureDto>>(procedureDto);
            return ApiResponse<List<ProcedureDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(ProcedureDto model)
        {
            throw new NotImplementedException();
        }
    }
}
