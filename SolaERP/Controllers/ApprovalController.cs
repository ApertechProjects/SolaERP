using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ApprovalController : ControllerBase
    {
        public ApprovalController(ConfHelper confHelper)
        {
            ConfHelper = confHelper;
        }

        public ConfHelper ConfHelper { get; }

        [HttpGet]
        public async Task<ApiResult> GetBUList([FromHeader] string token)
        {
            return await new EntityLogic(ConfHelper).GetBusinessUnitList(token);
        }

        [HttpGet("{buId}")]
        public async Task<ApiResult> GetApprovalStageLoad([FromHeader] string token, int buId)
        {
            return await new EntityLogic(ConfHelper).GetApprovalStageLoad(token, buId);
        }

        [HttpGet("{approvalStageMainId}")]
        public async Task<ApiResult> GetApprovalStageHeaderLoad([FromHeader] string token, int approvalStageMainId)
        {
            return await new EntityLogic(ConfHelper).GetApprovalStageHeaderLoad(token, approvalStageMainId);
        }

        [HttpGet("{approvalStageMainId}")]
        public async Task<ApiResult> GetApprovalStageDetailsLoad([FromHeader] string token, int approvalStageMainId)
        {
            return await new EntityLogic(ConfHelper).GetApprovalStageDetailsLoad(token, approvalStageMainId);
        }

        [HttpGet("{approvalStageDetailId}")]
        public async Task<ApiResult> GetApproveStageRoles_Load([FromHeader] string token, int approvalStageDetailId)
        {
            return await new EntityLogic(ConfHelper).GetApprovalStageRolesLoad(token, approvalStageDetailId);
        }

        [HttpGet]
        public async Task<ApiResult> GetApproveRoles([FromHeader] string token)
        {
            return await new EntityLogic(ConfHelper).GetApproveRoles(token);
        }

        [HttpGet]
        public async Task<ApiResult> GetProceduresList([FromHeader] string token)
        {
            return await new EntityLogic(ConfHelper).GetProceduresList(token);
        }

        [HttpPost]
        public async Task<ApiResult> SaveApprovalStage([FromHeader] string token, SaveApprovalStageWrapper saveApprovalStageWrapper)
        {
            return await new EntityLogic(ConfHelper).SaveApprovalStage(token, saveApprovalStageWrapper);
        }

    }
}
