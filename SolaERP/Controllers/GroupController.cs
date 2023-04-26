using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Entities.Groups;
using SolaERP.Infrastructure.Models;


namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class GroupController : CustomBaseController
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IBusinessUnitService _businessUnitService;
        public GroupController(IGroupService groupService, IUserService userService, IBusinessUnitService businessUnitService)
        {
            _groupService = groupService;
            _userService = userService;
            _businessUnitService = businessUnitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsAsync()
            => CreateActionResult(await _groupService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> GetEmailNotifications([FromQuery] int groupid)
           => CreateActionResult(await _groupService.GetGroupEmailNotificationsAsync(groupid));

        [HttpGet]
        public async Task<IActionResult> GetBusinessUnits([FromQuery] int groupId)
            => CreateActionResult(await _businessUnitService.GetBusinessUnitForGroupAsync(groupId));

        [HttpGet]
        public async Task<IActionResult> GetGroupInfoAsync([FromQuery] int groupId)
          => CreateActionResult(await _groupService.GetGroupInfoAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveGroupAsync(GroupSaveModel model)
            => CreateActionResult(await _groupService.SaveGroupAsync(User.Identity.Name, model));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetBuyers(int groupId)
           => CreateActionResult(await _groupService.GetBuyersByGroupIdAsync(groupId));

        [HttpGet]
        public async Task<IActionResult> GetGroupRoles(int groupId)
            => CreateActionResult(await _groupService.GetGroupRolesByGroupIdAsync(groupId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAdditionalPrivileges(int groupId)
          => CreateActionResult(await _groupService.GetAdditionalPrivilegesForGroupAsync(groupId));

        [HttpGet("{userId}")]
        public async Task<IActionResult> AvailableGroupsForUser(int userId)
          => CreateActionResult(await _groupService.GetUserGroupsWithoutCurrents(userId));

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int groupId)
          => CreateActionResult(await _userService.GetUsersByGroupIdAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveBuyerByGroupAsync(GroupBuyerSaveModel model)
            => CreateActionResult(await _groupService.SaveBuyerByGroupAsync(model));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAnalysisCodes(int groupId)
          => CreateActionResult(await _groupService.GetAnalysisCodesByGroupIdAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveAnalysisCodeByGroupAsync(AnalysisCodeSaveModel model)
            => CreateActionResult(await _groupService.SaveAnalysisCodeByGroupAsync(model));

        [HttpPost]
        public async Task<IActionResult> CreateEmailNotification(CreateGroupEmailNotificationModel model)
            => CreateActionResult(await _groupService.CreateEmailNotificationAsync(model));

        [HttpPut]
        public async Task<IActionResult> UpdateEmailNotification(GroupEmailNotification model)
            => CreateActionResult(await _groupService.UpdateEmailNotificationAsync(model));

        [HttpPost]
        public async Task<IActionResult> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model)
         => CreateActionResult(await _groupService.SaveGroupRoleByGroupAsync(model));

        [HttpDelete("{groupApproveRoleId}")]
        public async Task<IActionResult> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId)
            => CreateActionResult(await _groupService.DeleteGroupRoleByGroupIdAsync(groupApproveRoleId));

        [HttpDelete("{groupEmailNotificationId}")]
        public async Task<IActionResult> DeleteEmailNotification(int groupEmailNotificationId)
            => CreateActionResult(await _groupService.DeleteEmailNotificationAsync(groupEmailNotificationId));

        [HttpDelete("{groupBuyerId}")]
        public async Task<IActionResult> DeleteBuyerByGroupIdAsync(int groupBuyerId)
          => CreateActionResult(await _groupService.DeleteBuyerByGroupIdAsync(groupBuyerId));

        [HttpDelete("{groupAnalysisCodeId}")]
        public async Task<IActionResult> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId)
            => CreateActionResult(await _groupService.DeleteAnalysisCodeByGroupIdAsync(groupAnalysisCodeId));


    }
}
