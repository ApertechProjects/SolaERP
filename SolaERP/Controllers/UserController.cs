using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
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


        //[HttpGet]
        //public async Task<IActionResult> GetUserByToken()
        //    => CreateActionResult(await _userService.GetUserByTokenAsync(User.Identity.Name));

        //[HttpGet]
        //public async Task<string> GetUserNameByToken()
        //  => await _userService.GetUserNameByTokenAsync(User.Identity.Name);


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
        {
            return CreateActionResult(await _userService.GetUserWFAAsync(User.Identity.Name, userStatus, userType));
        }

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
        public async Task<IActionResult> UserChangeStatusAsync(UserChangeStatusModel model)
            => CreateActionResult(await _userService.UserChangeStatusAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> SaveUser(UserSaveModel user)
        {
            var result = await _userService.SaveUserAsync(user);

            if (Convert.ToBoolean(result.Data))
                return Ok(ApiResponse<NoContentDto>.Success(204));

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPasswordAsync(ChangeUserPasswordModel user)
            => CreateActionResult(await _userService.ChangeUserPasswordAsync(user));

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
            => CreateActionResult(await _userService.GetUserInfo(userId));

        [HttpGet]
        public async Task<IActionResult> GetERPUserAsync()
            => CreateActionResult(await _userService.GetERPUserAsync());

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] List<int> userIds)
            => CreateActionResult(await _userService.DeleteUserAsync(userIds));
    }
}
