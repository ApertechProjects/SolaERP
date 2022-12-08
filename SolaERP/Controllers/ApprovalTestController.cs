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
        private readonly IApproveStageRoleService _approveStageRoleService;
        public ApprovalTestController(IApproveStageMainService approveStageMainService, IApproveStageDetailService approveStageDetailService, IApproveStageRoleService approveStageRoleService)
        {
            _approveStageMainService = approveStageMainService;
            _approveStageDetailService = approveStageDetailService;
            _approveStageRoleService = approveStageRoleService;
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


    }

}
