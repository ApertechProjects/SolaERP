using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Dtos.Wrappers;
using SolaERP.Business.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GeneralController : CustomBaseController
    {
        private readonly IStatusService _statusService;
        public GeneralController(ConfHelper confHelper, IStatusService statusService)
        {
            ConfHelper = confHelper;
            _statusService = statusService;
        }

        public ConfHelper ConfHelper { get; }

        [HttpGet]
        public async Task<ApiResult> GetBUList()
        {
            return await new EntityLogic(ConfHelper).GetBUList(User.Identity.Name);
        }

        [HttpGet("{groupId}")]
        public async Task<ApiResult> GetBUListForFroup([FromHeader] string token, int groupId)
        {
            return await new EntityLogic(ConfHelper).GetBUListForGroup(token, groupId);
        }


        [HttpGet]
        public async Task<ApiResult> GetUserMenu_Load([FromHeader] string token)
        {
            return await new EntityLogic(ConfHelper).GetUserMenu_Load(token);
        }

        [HttpGet("{groupId}")]
        public async Task<ApiResult> GetUserMenu_LoadForGroup([FromHeader] string token, int groupId)
        {
            return await new EntityLogic(ConfHelper).GetUserMenu_LoadForGroup(token, groupId);
        }

        [HttpGet]
        public async Task<ApiResult> GetUserMenu_LoadWithoutAccess([FromHeader] string token)
        {
            return await new EntityLogic(ConfHelper).GetUserMenuWithoutAccess(token);
        }

        [HttpGet("{fileId}")]
        public async Task<ApiResult> DownloadFile([FromHeader] string token, int fileId)
        {
            return await new EntityLogic(ConfHelper).DownloadFile(token, fileId);
        }

        [HttpGet("{fileId}")]
        public async Task<ApiResult> DeleteFile([FromHeader] string token, int fileId)
        {
            return await new EntityLogic(ConfHelper).DeleteFile(token, fileId);
        }

        [HttpPost]
        public async Task<ApiResult> SaveFile([FromHeader] string token, SaveFileWrapper files)
        {
            return await new EntityLogic(ConfHelper).SaveFile(token, files);
        }

        [HttpGet("{groupId}")]
        public async Task<ApiResult> GetApproveRolesByGroupId([FromHeader] string token, int groupId)
        {
            return await new EntityLogic(ConfHelper).GetApproveRolesByGroupId(token, groupId);
        }

        [HttpGet("{groupId}")]
        public async Task<ApiResult> GetGroupAdditionalPrivileges([FromHeader] string token, int groupId)
        {
            return await new EntityLogic(ConfHelper).GetGroupAdditionalPrivileges(token, groupId);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusList()
           => CreateActionResult(await _statusService.GetAllAsync());
    }
}
