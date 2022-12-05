using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.Shared;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApprovalTestController : ControllerBase
    {
        private readonly IApproveStageMainService _approveStageMainService;
        public ApprovalTestController(IApproveStageMainService approveStageMainService)
        {
            _approveStageMainService = approveStageMainService;
        }

        [HttpGet("{buId}")]
        public async Task<ApiResponse<List<ApproveStagesMainDto>>> GetApproveStageMainByBusinessUnitId(int buId)
        {
            return await _approveStageMainService.GetByBusinessUnitId(buId);
        }
    }
}
+