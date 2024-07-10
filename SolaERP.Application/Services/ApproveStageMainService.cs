using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.Shared;

namespace SolaERP.Persistence.Services
{
    public class ApproveStageMainService : IApproveStageMainService, ILoggableCrudService<ApprovalStagesMainDto>
    {
        private readonly IApproveStageMainRepository _approveStageMainRepository;
        private IMapper _mapper;

        public ApproveStageMainService(IApproveStageMainRepository approveStageMainRepository, IMapper mapper)
        {
            _approveStageMainRepository = approveStageMainRepository;
            _mapper = mapper;
        }

        public Task AddAsync(ApprovalStagesMainDto model)
        {
            throw new NotImplementedException();
        }


        public Task<ApiResponse<List<ApprovalStagesMainDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<ApprovalStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approvalStageMainId)
        {
            var approveStageHeader = await _approveStageMainRepository.GetApprovalStageHeaderLoad(approvalStageMainId);
            var dto = _mapper.Map<ApprovalStagesMainDto>(approveStageHeader);
            return ApiResponse<ApprovalStagesMainDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ApprovalStagesMainDto>>> GetByBusinessUnitId(int buId)
        {
            var approveStagesList = await _approveStageMainRepository.GetByBusinessUnitId(buId);
            var dto = _mapper.Map<List<ApprovalStagesMainDto>>(approveStagesList);
            return ApiResponse<List<ApprovalStagesMainDto>>.Success(dto, 200);
        }

        public async Task<int> GetStageCount(int businessUnitId, string procedureKey)
        {
            var data = await _approveStageMainRepository.Stages(businessUnitId, procedureKey);
            return data.Count;
        }

        public Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApprovalStagesMainDto entity, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(ApprovalStagesMainDto entity, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(ApprovalStagesMainDto model)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(string authToken, ApprovalStagesMainDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(string authToken, ApprovalStagesMainDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
