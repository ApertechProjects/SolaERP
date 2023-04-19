using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [Authorize]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        public UserController(IUserService userService, IVendorService vendorService)
        {
            _userService = userService;
            _vendorService = vendorService;
        }


        /// <summary>
        ///Removes the user which referenced with authToken
        /// </summary>
        /// <remarks>Users who are authenticated and authorized to perform the action will be able to access the</remarks>
        /// <param name="authToken">userIdentifier which returns in Login or Register.</param>
        [HttpDelete]
        public async Task<IActionResult> RemoveUser()
            => CreateActionResult(await _userService.RemoveUserByTokenAsync(User.Identity.Name));

        /// <summary>
        ///Gets all active users for user list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveUsersAsync()
            => CreateActionResult(await _userService.GetActiveUsersAsync());

        [HttpGet]
        public async Task<IActionResult> GetActiveUsersWithoutCurrentUserAsync()
          => CreateActionResult(await _userService.GetActiveUsersWithoutCurrentUserAsync(User.Identity.Name));

        [HttpGet]
        public async Task<IActionResult> GetUserWFAAsync(int userStatus, int userType)
            => CreateActionResult(await _userService.GetUserWFAAsync(User.Identity.Name, userStatus, userType));

        [HttpGet]
        public async Task<IActionResult> GetUserAllAsync(int userStatus, int userType)
            => CreateActionResult(await _userService.GetUserAllAsync(User.Identity.Name, userStatus, userType));

        [HttpGet]
        public async Task<IActionResult> GetUserCompanyAsync([FromQuery] int userStatus)
            => CreateActionResult(await _userService.GetUserCompanyAsync(User.Identity.Name, userStatus));

        [HttpGet]
        public async Task<IActionResult> GetUserVendorAsync([FromQuery] int userStatus)
            => CreateActionResult(await _userService.GetUserVendorAsync(User.Identity.Name, userStatus));

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
        public async Task<IActionResult> DeleteUserAsync([FromBody] List<int> userIds)
            => CreateActionResult(await _userService.DeleteUserAsync(userIds));

        [HttpGet]
        public async Task<IActionResult> GetUsersByGroupIdAsync(int groupId)
            => CreateActionResult(await _userService.GetUsersByGroupIdAsync(groupId));
    }
}
