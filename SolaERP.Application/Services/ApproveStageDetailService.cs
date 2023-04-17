using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class ApproveStageDetailService : IApproveStageDetailService
    {
        private readonly IApproveStageDetailRepository _approveStageDetailsRepository;
        private readonly IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ApproveStageDetailService(IApproveStageDetailRepository approveStageDetailsRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _approveStageDetailsRepository = approveStageDetailsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task AddAsync(ApproveStagesDetailDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(string authToken, ApproveStagesDetailDto entity)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(authToken);
            var model = _mapper.Map<ApproveStagesDetail>(entity);
            var approveStageDetail = await _approveStageDetailsRepository.SaveDetailsAsync(model);
            return 0;
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

        public async Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            var approveDetail = _approveStageDetailsRepository.RemoveAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }



        public Task<ApiResponse<bool>> UpdateAsync(ApproveStagesDetailDto model)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(string authToken, ApproveStagesDetailDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
