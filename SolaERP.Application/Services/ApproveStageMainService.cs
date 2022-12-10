using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Application.Services
{
    public class ApproveStageMainService : IApproveStageMainService
    {
        private readonly IApproveStageMainRepository _approveStageMainRepository;
        private IMapper _mapper;

        public ApproveStageMainService(IApproveStageMainRepository approveStageMainRepository, IMapper mapper)
        {
            _approveStageMainRepository = approveStageMainRepository;
            _mapper = mapper;
        }

        public Task AddAsync(ApproveStagesMainDto model)
        {
            throw new NotImplementedException();
        }


        public Task<ApiResponse<List<ApproveStagesMainDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<ApproveStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approvalStageMainId)
        {
            var approveStageHeader = await _approveStageMainRepository.GetApprovalStageHeaderLoad(approvalStageMainId);
            var dto = _mapper.Map<ApproveStagesMainDto>(approveStageHeader);
            return ApiResponse<ApproveStagesMainDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ApproveStagesMainDto>>> GetByBusinessUnitId(int buId)
        {
            var approveStagesList = await _approveStageMainRepository.GetByBusinessUnitId(buId);
            var dto = _mapper.Map<List<ApproveStagesMainDto>>(approveStagesList);
            return ApiResponse<List<ApproveStagesMainDto>>.Success(dto, 200);
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproveStagesMainDto entity, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(ApproveStagesMainDto entity, int userId = 0)
        {
            var model = _mapper.Map<ApproveStagesMain>(entity);
            var approveStageMain = await _approveStageMainRepository.AddAsync(model, userId);
            return approveStageMain;
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApproveStagesMainDto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(ApproveStagesMainDto entity, string authToken)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproveStagesMainDto entity, string authToken)
        {
            throw new NotImplementedException();
        }
    }
}
