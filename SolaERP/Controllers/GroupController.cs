using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;


namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class GroupController : CustomBaseController
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        public GroupController(IGroupService groupService, IUserService userService, IBusinessUnitService businessUnitService)
        {
            _groupService = groupService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsAsync()
            => CreateActionResult(await _groupService.GetAllAsync());

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetEmailNotifications(int groupId)
           => CreateActionResult(await _groupService.GetGroupEmailNotificationsAsync(groupId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetBusinessUnits(int groupId)
            => CreateActionResult(await _groupService.GetGroupBusinessUnitsAsync(groupId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroupInfoAsync(int groupId)
          => CreateActionResult(await _groupService.GetGroupInfoAsync(groupId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAnalysisCodes(int groupId)
       => CreateActionResult(await _groupService.GetAnalysisCodesAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveGroupAsync(GroupSaveModel model)
            => CreateActionResult(await _groupService.SaveGroupAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> DeleteGroupAsync(GroupDeleteModel models)
            => CreateActionResult(await _groupService.DeleteGroupAsync(User.Identity.Name, models));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetBuyers(int groupId)
           => CreateActionResult(await _groupService.GetGroupBuyersAsync(groupId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetRoles(int groupId)
            => CreateActionResult(await _groupService.GetGroupRolesAsync(groupId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAdditionalPrivileges(int groupId)
          => CreateActionResult(await _groupService.GetAdditionalPrivilegesAsync(groupId));

        [HttpGet("{userId}")]
        public async Task<IActionResult> AvailableGroupsForUser(int userId)
          => CreateActionResult(await _groupService.GetUserGroupsWithoutCurrents(userId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetUsers(int groupId)
          => CreateActionResult(await _userService.GetUsersByGroupIdAsync(groupId));

    }
}
