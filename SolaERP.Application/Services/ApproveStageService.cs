using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveStage;
using SolaERP.Application.Dtos.ApproveStages;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.ApproveStage;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;

namespace SolaERP.Persistence.Services
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



        public async Task<ApiResponse<ApproveStagesMainDto>> GetMainByIdAsync(int approvalStageMainId)
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

        public async Task<ApiResponse<ApprovalStageSaveModel>> SaveApproveStageMainAsync(string name, ApprovalStageSaveModel approvalStageSaveVM)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var mainId = await _approveStageMainRepository.AddAsync(_mapper.Map<ApproveStagesMain>(approvalStageSaveVM.ApproveStagesMainDto), userId);
            approvalStageSaveVM.ApproveStagesMainDto.ApproveStageMainId = mainId;
            for (int i = 0; i < approvalStageSaveVM.ApproveStagesDetailDtos.Count; i++)
            {
                if (approvalStageSaveVM.ApproveStagesDetailDtos[i].Type == "remove")
                    await _approveStageDetailRepository.RemoveAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageDetailsId);
                else
                {
                    approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageMainId = mainId;
                    var detailId = await _approveStageDetailRepository.SaveDetailsAsync(_mapper.Map<ApproveStagesDetail>(approvalStageSaveVM.ApproveStagesDetailDtos[i]));

                    for (int j = 0; j < approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRoles.Count; j++)
                    {
                        if (approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRoles[j].Type == "remove")
                            await _approveStageRoleRepository.RemoveAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRoles[j].ApproveStageRoleId);
                        else
                        {

                            approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRoles[j].ApproveStageDetailId = detailId;
                            await _approveStageRoleRepository.AddAsync(_mapper.Map<ApproveStageRole>(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRoles[j]));
                        }
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

            return ApiResponse<List<ApprovalStatusDto>>.Fail("get", "Approval status is empty", 404, true);
        }

        public async Task<ApiResponse<bool>> DeleteApproveStageAsync(ApproveStageDeleteModel model)
        {
            var data = false;
            int counter = 0;
            for (int i = 0; i < model.stageIds.Count; i++)
            {
                data = await _approveStageMainRepository.DeleteApproveStageAsync(model.stageIds[i]);
                if (data)
                    counter++;
            }
            if (counter == model.stageIds.Count)
            {
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(data, 200);
            }
            return ApiResponse<bool>.Fail("delete", "data can not be deleted", 400);
        }
    }
}
