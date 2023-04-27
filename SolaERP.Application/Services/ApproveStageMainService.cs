using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class ApproveStageMainService : IApproveStageMainService, ILoggableCrudService<ApproveStagesMainDto>
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

        public Task<bool> AddAsync(ApproveStagesMainDto entity, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApproveStagesMainDto model)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(string authToken, ApproveStagesMainDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(string authToken, ApproveStagesMainDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
