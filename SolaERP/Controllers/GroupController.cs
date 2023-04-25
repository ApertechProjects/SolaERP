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
        public GroupController(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsAsync()
            => CreateActionResult(await _groupService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> GetGroupsByUserIdAsync(int userId)
           => CreateActionResult(await _groupService.GetGroupsByUserIdAsync(userId));

        [HttpGet]
        public async Task<IActionResult> GetGroupInfoAsync(int groupId)
          => CreateActionResult(await _groupService.GetGroupInfoAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveGroupAsync(GroupSaveModel model)
            => CreateActionResult(await _groupService.SaveGroupAsync(User.Identity.Name, model));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetBuyersByGroupIdAsync(int groupId)
           => CreateActionResult(await _groupService.GetBuyersByGroupIdAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveBuyerByGroupAsync(GroupBuyerSaveModel model)
            => CreateActionResult(await _groupService.SaveBuyerByGroupAsync(model));

        [HttpDelete("{groupBuyerId}")]
        public async Task<IActionResult> DeleteBuyerByGroupIdAsync(int groupBuyerId)
            => CreateActionResult(await _groupService.DeleteBuyerByGroupIdAsync(groupBuyerId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAnalysisCodesByGroupIdAsync(int groupId)
          => CreateActionResult(await _groupService.GetAnalysisCodesByGroupIdAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveAnalysisCodeByGroupAsync(AnalysisCodeSaveModel model)
            => CreateActionResult(await _groupService.SaveAnalysisCodeByGroupAsync(model));

        [HttpDelete("{groupAnalysisCodeId}")]
        public async Task<IActionResult> DeleteAnalysisCodeByGroupIdAsync(int groupAnalysisCodeId)
            => CreateActionResult(await _groupService.DeleteAnalysisCodeByGroupIdAsync(groupAnalysisCodeId));

        [HttpGet]
        public async Task<IActionResult> GetGroupRolesByGroupIdAsync(int groupId)
            => CreateActionResult(await _groupService.GetGroupRolesByGroupIdAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> SaveGroupRoleByGroupAsync(GroupRoleSaveModel model)
         => CreateActionResult(await _groupService.SaveGroupRoleByGroupAsync(model));

        [HttpDelete("{groupApproveRoleId}")]
        public async Task<IActionResult> DeleteGroupRoleByGroupIdAsync(int groupApproveRoleId)
            => CreateActionResult(await _groupService.DeleteGroupRoleByGroupIdAsync(groupApproveRoleId));

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAdditionalPrivilegesByGroup(int groupId)
            => CreateActionResult(await _groupService.GetAdditionalPrivilegesForGroupAsync(groupId));

        [HttpGet("{userId}")]
        public async Task<IActionResult> AvailableGroupsForUser(int userId)
          => CreateActionResult(await _groupService.GetUserGroupsWithoutCurrents(userId));

        [HttpDelete("{groupEmailNotificationId}")]
        public async Task<IActionResult> DeleteEmailNotification(int groupEmailNotificationId)
            => CreateActionResult(await _groupService.DeleteEmailNotificationAsync(groupEmailNotificationId));

        [HttpGet("{groupid}")]
        public async Task<IActionResult> GetEmailNotifications(int groupid)
            => CreateActionResult(await _groupService.GetGroupEmailNotificationsAsync(groupid));

        [HttpPost]
        public async Task<IActionResult> CreateEmailNotification(CreateGroupEmailNotificationModel model)
            => CreateActionResult(await _groupService.CreateEmailNotificationAsync(model));

        [HttpPut]
        public async Task<IActionResult> UpdateEmailNotification(GroupEmailNotification model)
            => CreateActionResult(await _groupService.UpdateEmailNotificationAsync(model));

        [HttpPost]
        public async Task<IActionResult> AddUserToGroupAsync(List<AddUserToGroupModel> model)
            => CreateActionResult(await _groupService.AddUserToGroupAsync(model));

        [HttpDelete]
        public async Task<IActionResult> DeleteUserFromGroupAsync(List<int> groupUserIds)
            => CreateActionResult(await _groupService.DeleteUserFromGroupAsync(groupUserIds));

        [HttpGet]
        public async Task<IActionResult> GetUsersByGroupIdAsync(int groupId)
           => CreateActionResult(await _userService.GetUsersByGroupIdAsync(groupId));

    }
}
