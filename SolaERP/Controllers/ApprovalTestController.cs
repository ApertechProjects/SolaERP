using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveRole;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Procedure;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ApprovalTestController : ControllerBase
    {
        private readonly IApproveStageMainService _approveStageMainService;
        private readonly IApproveStageDetailService _approveStageDetailService;
        private readonly IApproveStageRoleService _approveStageRoleService;
        private readonly IApproveRoleService _approveRoleService;
        private readonly IProcedureService _procedureService;
        public ApprovalTestController(IApproveStageMainService approveStageMainService,
                                      IApproveStageDetailService approveStageDetailService,
                                      IApproveStageRoleService approveStageRoleService,
                                      IApproveRoleService approveRoleService,
                                      IProcedureService procedureService)
        {
            _approveStageMainService = approveStageMainService;
            _approveStageDetailService = approveStageDetailService;
            _approveStageRoleService = approveStageRoleService;
            _approveRoleService = approveRoleService;
            _procedureService = procedureService;
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
    }

}
