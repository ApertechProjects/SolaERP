using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        private readonly IGroupService _groupService;
        public UserController(IUserService userService, IVendorService vendorService)
        {
            _userService = userService;
            _vendorService = vendorService;
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveUser()
            => CreateActionResult(await _userService.RemoveUserByTokenAsync(User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetGroupsByUserIdAsync([FromQuery] int userId)
         => CreateActionResult(await _groupService.GetGroupsByUserIdAsync(userId));

        [HttpGet]
        public async Task<IActionResult> GetActiveUsersAsync()
            => CreateActionResult(await _userService.GetActiveUsersAsync());

        [HttpGet]
        public async Task<IActionResult> GetActiveUsersWithoutCurrentUserAsync()
          => CreateActionResult(await _userService.GetActiveUsersWithoutCurrentUserAsync(User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetUserWFAAsync(int userStatus, int userType, int page, int limit)
            => CreateActionResult(await _userService.GetUserWFAAsync(User.Identity.Name, userStatus, userType, page, limit));

        [HttpGet]
        public async Task<IActionResult> GetUserAllAsync(int userStatus, int userType, int page, int limit)
        => CreateActionResult(await _userService.GetUserAllAsync(User.Identity.Name, userStatus, userType, page, limit));

        [HttpGet]
        public async Task<IActionResult> GetUserCompanyAsync([FromQuery] int userStatus, int page, int limit)
            => CreateActionResult(await _userService.GetUserCompanyAsync(User.Identity.Name, userStatus, page, limit));

        [HttpGet]
        public async Task<IActionResult> GetUserVendorAsync([FromQuery] int userStatus, int page, int limit)
            => CreateActionResult(await _userService.GetUserVendorAsync(User.Identity.Name, userStatus, page, limit));

        [HttpPost]
        public async Task<IActionResult> UserChangeStatusAsync(List<UserChangeStatusModel> model)
          => CreateActionResult(await _userService.UserChangeStatusAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> SaveUserAsync(UserSaveModel user)
            => CreateActionResult(await _userService.SaveUserAsync(user));

        [HttpPost]
        public async Task<IActionResult> ChangeUserPasswordAsync(ChangeUserPasswordModel user)
            => CreateActionResult(await _userService.ChangeUserPasswordAsync(user));

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfoAsync(int userId)
            => CreateActionResult(await _userService.GetUserInfoAsync(userId));

        [HttpGet]
        public async Task<IActionResult> GetERPUserAsync()
            => CreateActionResult(await _userService.GetERPUserAsync());

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(DeleteUser deleteUser)
            => CreateActionResult(await _userService.DeleteUserAsync(deleteUser));

        [HttpGet]
        public async Task<IActionResult> GetUsersByGroupIdAsync(int groupId)
            => CreateActionResult(await _userService.GetUsersByGroupIdAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> AddGroupToUserAsync(List<int> groupsIds, int userId)
            => CreateActionResult(await _userService.AddGroupToUserAsync(groupsIds, userId));

        [HttpPost]
        public async Task<IActionResult> DeleteGroupFromUserAsync(List<int> groupsIds, int userId)
          => CreateActionResult(await _userService.DeleteGroupFromUserAsync(groupsIds, userId));

    }
}
