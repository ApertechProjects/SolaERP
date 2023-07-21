using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
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


        public async Task<ApiResponse<List<ApprovalStagesDetailDto>>> GetDetailByIdAsync(int approveStageMainId)
        {
            var approveDetailById = await _approveStageDetailsRepository.GetByMainIdAsync(approveStageMainId);
            var dto = _mapper.Map<List<ApprovalStagesDetailDto>>(approveDetailById);
            return ApiResponse<List<ApprovalStagesDetailDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            var approveDetail = _approveStageDetailsRepository.RemoveAsync(Id);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }



    }
}
