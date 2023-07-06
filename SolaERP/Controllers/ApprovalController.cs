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
        public async Task<IActionResult> Stage(int buId)
            => CreateActionResult(await _approveStageMainService.GetByBusinessUnitId(buId));

        [HttpGet("{mainId}")]
        public async Task<IActionResult> ApprovalStage(int mainId)
           => CreateActionResult(await _approveStageMainService.GetApprovalStageAsync(mainId));

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> Roles(int businessUnitId)
            => CreateActionResult(await _approveRoleService.ApproveRoleAsync(businessUnitId));

        [HttpGet]
        public async Task<IActionResult> Procedures()
            => CreateActionResult(await _procedureService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> SaveStage(ApprovalStageSaveModel approvalStageSaveVM)
            => CreateActionResult(await _approveStageMainService.SaveApproveStageMainAsync(User.Identity.Name, approvalStageSaveVM));

        [HttpGet]
        public async Task<IActionResult> ApprovalStatus()
            => CreateActionResult(await _approveStageMainService.GetApproveStatus());

        [HttpPost]
        public async Task<IActionResult> SaveRole(List<ApproveRoleSaveModel> model)
            => CreateActionResult(await _approveRoleService.ApproveRoleSaveAsync(model, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(ApproveRoleDeleteModel model)
        => CreateActionResult(await _approveRoleService.ApproveRoleDeleteAsync(model, User.Identity.Name));

        [HttpDelete]
        public async Task<IActionResult> DeleteStage(ApproveStageDeleteModel model)
            => CreateActionResult(await _approveStageMainService.DeleteApproveStageAsync(model));

    }
}
