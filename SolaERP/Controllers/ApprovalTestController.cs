using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Procedure;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApprovalTestController : ControllerBase
    {
        private readonly IApproveStageService _approveStageMainService;
        private readonly IApproveStageDetailService _approveStageDetailService;
        private readonly IApproveStageRoleService _approveStageRoleService;
        private readonly IApproveRoleService _approveRoleService;
        private readonly IProcedureService _procedureService;
        private readonly IUserService _userService;
        public ApprovalTestController(IApproveStageService approveStageMainService,
                                      IApproveStageDetailService approveStageDetailService,
                                      IApproveStageRoleService approveStageRoleService,
                                      IApproveRoleService approveRoleService,
                                      IProcedureService procedureService,
                                      IUserService userService)
        {
            _approveStageMainService = approveStageMainService;
            _approveStageDetailService = approveStageDetailService;
            _approveStageRoleService = approveStageRoleService;
            _approveRoleService = approveRoleService;
            _procedureService = procedureService;
            _userService = userService;
        }

        [HttpGet("{buId}")]
        public async Task<ApiResponse<List<ApproveStagesMainDto>>> GetApproveStageMainByBuId(int buId)
        {
            return await _approveStageMainService.GetByBusinessUnitId(buId);
        }

        [HttpGet("{approveStageMainId}")]
        public async Task<ApiResponse<ApproveStagesMainDto>> GetApproveStageMainByApprovalStageMainId(int approveStageMainId)
        {
            return await _approveStageMainService.GetApproveStageMainByApprovalStageMainId(approveStageMainId);
        }

        [HttpGet("{approveStageMainId}")]
        public async Task<ApiResponse<List<ApproveStagesDetailDto>>> GetApproveStageDetailsByApprovalStageMainId(int approveStageMainId)
        {
            return await _approveStageDetailService.GetApproveStageDetailsByApproveStageMainId(approveStageMainId);
        }

        [HttpGet("{approveStageDetailId}")]
        public async Task<ApiResponse<List<ApproveStageRoleDto>>> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailId)
        {
            return await _approveStageRoleService.GetApproveStageRolesByApproveStageDetailId(approveStageDetailId);
        }

        [HttpGet]
        public async Task<ApiResponse<List<ApproveRoleDto>>> GetApproveRoles()
        {
            return await _approveRoleService.GetAllAsync();
        }

        [HttpGet]
        public async Task<ApiResponse<List<ProcedureDto>>> GetProcedures()
        {
            return await _procedureService.GetAllAsync();
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> ApprovalStageSave([FromHeader] string authToken, ApprovalStageSaveVM approvalStageSaveVM)
        {
            string typeOfDetail = string.Empty;
            string typeOfRole = string.Empty;

            await _approveStageMainService.AddAsync(authToken, approvalStageSaveVM.ApproveStagesMainDto);

            //for (int i = 0; i < approvalStageSaveVM.ApproveStagesDetailDtos.Count; i++)
            //{
            //    typeOfDetail = approvalStageSaveVM.ApproveStagesDetailDtos[i].Type;
            //    if (typeOfDetail == "remove")
            //    {
            //        await _approveStageDetailService.RemoveAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageDetailsId); //+
            //    }
            //    else
            //    {
            //        await _approveStageDetailService.AddAsync(authToken, approvalStageSaveVM.ApproveStagesDetailDtos[i]);

            //        for (int j = 0; j < approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto.Count; j++)
            //        {
            //            typeOfRole = approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j].Type;

            //            if (typeOfRole == "remove")
            //            {
            //                await _approveStageRoleService.RemoveAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j].ApproveStageRoleId);
            //            }
            //            else
            //            {
            //                await _approveStageRoleService.AddAsync(approvalStageSaveVM.ApproveStagesDetailDtos[i].ApproveStageRolesDto[j]);
            //            }
            //        }
            //    }
            //}
            return ApiResponse<bool>.Success(200);
        }
    }

}
