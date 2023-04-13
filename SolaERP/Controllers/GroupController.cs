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
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsAsync()
            => CreateActionResult(await _groupService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> GetGroupsByUserIdAsync(int userId)
           => CreateActionResult(await _groupService.GetGroupsByUserIdAsync(userId));

        [HttpPost]
        public async Task<IActionResult> SaveGroupAsync([FromHeader] string authToken, GroupSaveModel model)
            => CreateActionResult(await _groupService.SaveGroupAsync(authToken, model));

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

        [HttpDelete("{groupEmailNotficationId}")]
        public async Task<IActionResult> DeleteEmailNotfication(int groupEmailNotficationId)
            => CreateActionResult(await _groupService.DeleteEmailNotficationAsync(groupEmailNotficationId));

        [HttpGet("{groupid}")]
        public async Task<IActionResult> GetEmailNotfications(int groupid)
            => CreateActionResult(await _groupService.GetGroupEmailNotficationsAsync(groupid));

        [HttpPost]
        public async Task<IActionResult> CreateEmailNotfication(CreateGroupEmailNotficationModel model)
            => CreateActionResult(await _groupService.CreateEmailNotficationAsync(model));

        [HttpPut]
        public async Task<IActionResult> UpdateEmailNotfication(GroupEmailNotfication model)
            => CreateActionResult(await _groupService.UpdateEmailNotficationAsync(model));

    }
}
