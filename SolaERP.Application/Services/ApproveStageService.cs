using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class ApproveStageService : IApproveStageService
    {
        private readonly IApproveStageMainRepository _approveStageMainRepository;
        private readonly IApproveStageDetailRepository _approveStageDetailRepository;
        private readonly IApproveStageRoleRepository _approveStageRoleRepository;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        public ApproveStageService(IApproveStageMainRepository approveStageMainRepository,
                                   IApproveStageDetailRepository approveStageDetailRepository,
                                   IUserRepository userRepository,
                                   IMapper mapper, IUnitOfWork unitOfWork,
                                   IApproveStageRoleRepository approveStageRoleRepository)
        {
            _approveStageMainRepository = approveStageMainRepository;
            _approveStageDetailRepository = approveStageDetailRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _approveStageRoleRepository = approveStageRoleRepository;
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

        public async Task<ApiResponse<ApprovalStageSaveModel>> SaveApproveStageMainAsync(string authToken, ApprovalStageSaveModel approvalStageSaveVM)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            await _approveStageMainRepository.AddAsync(_mapper.Map<ApproveStagesMain>(approvalStageSaveVM.ApproveStagesMainDto), userId);

            for (int i = 0; i < approvalStageSaveVM.ApproveStagesDetailDtos.Count; i++)
            {
                if (approvalStageSaveVM.ApproveStagesDetailDtos[i].Type == "remove")
                    await _approveStageDetailRepository.RemoveAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageDetailsId);
                else
                {
                    await _approveStageDetailRepository.SaveDetailsAsync(_mapper.Map<ApproveStagesDetail>(approvalStageSaveVM.ApproveStagesDetailDtos[i]));

                    for (int j = 0; j < approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto.Count; j++)
                    {
                        if (approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j].Type == "remove")
                            await _approveStageRoleRepository.RemoveAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j].ApproveStageRoleId);
                        else
                            await _approveStageRoleRepository.AddAsync(_mapper.Map<ApproveStageRole>(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j]));
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<ApprovalStageSaveModel>.Success(approvalStageSaveVM, 200);
        }

        public async Task<ApiResponse<List<ApprovalStatusDto>>> GetApproveStatus()
        {
            var approvalStatuses = await _approveStageMainRepository.GetApprovalStatusList();
            var approvalStatusDto = _mapper.Map<List<ApprovalStatusDto>>(approvalStatuses);

            if (approvalStatusDto.Any())
                return ApiResponse<List<ApprovalStatusDto>>.Success(approvalStatusDto, 200);

            return ApiResponse<List<ApprovalStatusDto>>.Fail("Approval status is empty", 404);
        }
    }
}
