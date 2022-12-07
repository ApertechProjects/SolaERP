using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Business.Models;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.ApproveStage;
using SolaERP.Infrastructure.Dtos.ApproveStages;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.ApproveStage;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApprovalTestController : ControllerBase
    {
        private readonly IApproveStageMainService _approveStageMainService;
        private readonly IApproveStageDetailService _approveStageDetailService;
        public ApprovalTestController(IApproveStageMainService approveStageMainService, IApproveStageDetailService approveStageDetailService)
        {
            _approveStageMainService = approveStageMainService;
            _approveStageDetailService = approveStageDetailService;
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
    }

}
