using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Application.Services
{
    public class ApproveStageService : IApproveStageService
    {
        private readonly IApproveStageMainRepository _approveStageMainRepository;
        private readonly IApproveStageDetailRepository _approveStageDetailRepository;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public ApproveStageService(IApproveStageMainRepository approveStageMainRepository, IUserRepository userRepository, IMapper mapper)
        {
            _approveStageMainRepository = approveStageMainRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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

        public async Task<int> UpdateAsync(string authToken, ApproveStagesMainDto entity)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var model = _mapper.Map<ApproveStagesMain>(entity);
            var approveStageMain = await _approveStageMainRepository.UpdateAsync(model, userId);
            return approveStageMain;
        }

        public async Task<int> AddAsync(string authToken, ApproveStagesMainDto entity)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var model = _mapper.Map<ApproveStagesMain>(entity);
            var approveStageMain = await _approveStageMainRepository.AddAsync(model, userId);
            return approveStageMain;
        }

        public async Task<int> SaveApproveStageDetailsAsync(List<ApproveStagesDetailDto> details)
        {
            var entityList = _mapper.Map<List<ApproveStagesDetail>>(details);
            foreach (var item in collection)
            {
                var approveStageDetail = await _approveStageDetailRepository.AddAsync(details, userId);

            }
            return approveStageDetail;
        }

        public Task<int> RemoveApproveStageDetailsAsync(int approveStageDetailsId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveApproveStageRolesAsync(List<ApproveStageRoleDto> roles)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveApproveStageolesAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId)
        {
            throw new NotImplementedException();
        }
    }
}
