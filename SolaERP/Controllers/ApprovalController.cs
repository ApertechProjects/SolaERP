using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.ApproveRole;
using SolaERP.Application.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ApprovalController : CustomBaseController
    {
        private readonly IApproveStageService _approveStageMainService;
        private readonly IApproveStageDetailService _approveStageDetailService;
        private readonly IApproveStageRoleService _approveStageRoleService;
        private readonly IApproveRoleService _approveRoleService;
        private readonly IProcedureService _procedureService;
        private readonly IUserService _userService;
        public ApprovalController(IApproveStageService approveStageMainService,
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
        public async Task<IActionResult> GetApproveStageMainByBuIdAsync(int buId)
            => CreateActionResult(await _approveStageMainService.GetByBusinessUnitId(buId));

        [HttpGet("{approveStageMainId}")]
        public async Task<IActionResult> GetApproveStageMainByApprovalStageMainIdAsync(int approveStageMainId)
            => CreateActionResult(await _approveStageMainService.GetApproveStageMainByApprovalStageMainId(approveStageMainId));

        [HttpGet("{approveStageMainId}")]
        public async Task<IActionResult> GetApproveStageDetailsByApprovalStageMainIdAsync(int approveStageMainId)
            => CreateActionResult(await _approveStageDetailService.GetApproveStageDetailsByApproveStageMainId(approveStageMainId));

        [HttpGet("{approveStageDetailId}")]
        public async Task<IActionResult> GetApproveStageRolesByApproveStageDetailIdAsync(int approveStageDetailId)
            => CreateActionResult(await _approveStageRoleService.GetApproveStageRolesByApproveStageDetailId(approveStageDetailId));

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> GetApproveRolesAsync(int businessUnitId)
            => CreateActionResult(await _approveRoleService.ApproveRoleAsync(businessUnitId));

        [HttpGet]
        public async Task<IActionResult> GetProceduresAsync()
            => CreateActionResult(await _procedureService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> ApprovalStageSaveAsync(ApprovalStageSaveModel approvalStageSaveVM)
            => CreateActionResult(await _approveStageMainService.SaveApproveStageMainAsync(User.Identity.Name, approvalStageSaveVM));

        [HttpGet]
        public async Task<IActionResult> GetApprovalStatusAsync()
            => CreateActionResult(await _approveStageMainService.GetApproveStatus());

        [HttpPost]
        public async Task<IActionResult> ApproveRoleSaveAsync(List<ApproveRoleSaveModel> model)
            => CreateActionResult(await _approveRoleService.ApproveRoleSaveAsync(model, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> ApproveRoleDeleteAsync(int roleId)
        => CreateActionResult(await _approveRoleService.ApproveRoleDeleteAsync(roleId, User.Identity.Name));

        [HttpDelete("{approveStageMainId}")]
        public async Task<IActionResult> DeleteApproveStageAsync(int approveStageMainId)
            => CreateActionResult(await _approveStageMainService.DeleteApproveStageAsync(approveStageMainId));

    }
}
