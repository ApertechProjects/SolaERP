using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class GroupController : CustomBaseController
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsAsync()
            => CreateActionResult(await _groupService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> SaveGroupAsync([FromHeader] string authToken, GroupSaveModel model)
            => CreateActionResult(await _groupService.SaveGroupAsync(authToken, model));

    }
}
