using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ApprovalTestController : CustomBaseController
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
        public async Task<IActionResult> GetApproveStageMainByBuId(int buId)
            => CreateActionResult(await _approveStageMainService.GetByBusinessUnitId(buId));

        [HttpGet("{approveStageMainId}")]
        public async Task<IActionResult> GetApproveStageMainByApprovalStageMainId(int approveStageMainId)
            => CreateActionResult(await _approveStageMainService.GetApproveStageMainByApprovalStageMainId(approveStageMainId));

        [HttpGet("{approveStageMainId}")]
        public async Task<IActionResult> GetApproveStageDetailsByApprovalStageMainId(int approveStageMainId)
            => CreateActionResult(await _approveStageDetailService.GetApproveStageDetailsByApproveStageMainId(approveStageMainId));

        [HttpGet("{approveStageDetailId}")]
        public async Task<IActionResult> GetApproveStageRolesByApproveStageDetailId(int approveStageDetailId)
            => CreateActionResult(await _approveStageRoleService.GetApproveStageRolesByApproveStageDetailId(approveStageDetailId));

        [HttpGet]
        public async Task<IActionResult> GetApproveRoles()
            => CreateActionResult(await _approveRoleService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> GetProcedures()
            => CreateActionResult(await _procedureService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> ApprovalStageSave([FromHeader] string authToken, ApprovalStageSaveVM approvalStageSaveVM)
            => CreateActionResult(await _approveStageMainService.SaveApproveStageMainAsync(authToken, approvalStageSaveVM));

        [HttpGet]
        public async Task<IActionResult> GetApprovalStutuses()
            => CreateActionResult(await _approveStageMainService.GetApproveStatuses());
    }

}
