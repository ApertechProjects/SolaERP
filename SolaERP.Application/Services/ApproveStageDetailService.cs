using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Application.Services
{
    public class ApproveStageDetailService : IApproveStageDetailService
    {
        private readonly IApproveStageDetailRepository _approveStageDetailsRepository;
        private IMapper _mapper;
        public ApproveStageDetailService(IApproveStageDetailRepository approveStageDetailsRepository, IMapper mapper)
        {
            _approveStageDetailsRepository = approveStageDetailsRepository;
            _mapper = mapper;
        }

        public Task AddAsync(ApproveStagesDetailDto model)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<ApproveStagesDetailDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId)
        {
            var approveDetailById = await _approveStageDetailsRepository.GetApproveStageDetailsByApproveStageMainId(approveStageMainId);
            var dto = _mapper.Map<List<ApproveStagesDetailDto>>(approveDetailById);
            return ApiResponse<List<ApproveStagesDetailDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApproveStagesDetailDto model)
        {
            throw new NotImplementedException();
        }
    }
}
