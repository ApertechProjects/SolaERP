using AutoMapper;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;
using SolaERP.Infrastructure.UnitOfWork;
using SolaERP.Infrastructure.ViewModels;

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

        public ApproveStageService(IApproveStageMainRepository approveStageMainRepository, IApproveStageDetailRepository approveStageDetailRepository, IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _approveStageMainRepository = approveStageMainRepository;
            _approveStageDetailRepository = approveStageDetailRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.SaveChangesAsync();
            return approveStageMain;
        }

        public async Task<int> SaveApproveStageDetailsAsync(ApproveStagesDetailDto detail)
        {
            var entity = _mapper.Map<ApproveStagesDetail>(detail);
            var approveStageDetail = await _approveStageDetailRepository.AddAsync(entity);
            return approveStageDetail;
        }

        public bool RemoveApproveStageDetailsAsync(int approveStageDetailsId)
        {
            var approveDetail = _approveStageDetailRepository.RemoveAsync(approveStageDetailsId);
            return approveDetail;
        }

        public async Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApproveStageMainId(int approveStageMainId)
        {
            var approveDetailById = await _approveStageDetailRepository.GetApproveStageDetailsByApproveStageMainId(approveStageMainId);
            var dto = _mapper.Map<List<ApproveStagesDetailDto>>(approveDetailById);
            return ApiResponse<List<ApproveStagesDetailDto>>.Success(dto, 200);
        }

        public async Task<int> SaveApproveStageRolesAsync(ApproveStageRoleDto role)
        {
            var entity = _mapper.Map<ApproveStageRole>(role);
            var model = await _approveStageRoleRepository.AddAsync(entity);
            return model;
        }

        public bool RemoveApproveStageRolesAsync(int roleId)
        {
            var approveStageRole = _approveStageRoleRepository.RemoveAsync(roleId);
            return approveStageRole;
        }

        public Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailsId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<ApprovalStageSaveVM>> SaveApproveStageMainAsync(string authToken, ApprovalStageSaveVM approvalStageSaveVM)
        {
            string typeOfDetail = string.Empty;
            string typeOfRole = string.Empty;

            await AddAsync(authToken, approvalStageSaveVM.ApproveStagesMainDto);

            for (int i = 0; i < approvalStageSaveVM.ApproveStagesDetailDtos.Count; i++)
            {
                typeOfDetail = approvalStageSaveVM.ApproveStagesDetailDtos[i].Type;
                if (typeOfDetail == "remove")
                    RemoveApproveStageDetailsAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageDetailsId); //+
                else
                {
                    await SaveApproveStageDetailsAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i]);

                    for (int j = 0; j < approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto.Count; j++)
                    {
                        typeOfRole = approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j].Type;

                        if (typeOfRole == "remove")
                            RemoveApproveStageRolesAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j].ApproveStageRoleId);
                        else
                            await SaveApproveStageRolesAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j]);
                    }
                }
            }

            return ApiResponse<ApprovalStageSaveVM>.Success(approvalStageSaveVM, 200);
        }
    }
}
