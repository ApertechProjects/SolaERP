using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SolaERP.API.Methods;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.ViewModels;
using SolaERP.Persistence.Services;
using System.Text.RegularExpressions;
using System.Web;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IFileUploadService _fileService;
        private readonly IGroupService _groupService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IMailService _mailService;
        public UserController(IUserService userService,
                              IFileUploadService fileService,
                              IGroupService groupService,
                              ITokenHandler tokenHandler,
                              IEmailNotificationService emailNotificationService,
                              IMailService mailService)
        {
            _userService = userService;
            _fileService = fileService;
            _groupService = groupService;
            _tokenHandler = tokenHandler;
            _emailNotificationService = emailNotificationService;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsByUserIdAsync([FromQuery] int userId)
         => CreateActionResult(await _groupService.GetUserGroupsAsync(userId));

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

        [HttpPost]
        public async Task<IActionResult> ChangeUserLanguage(string language)
            => CreateActionResult(await _userService.ChangeUserLanguage(User.Identity.Name, language));

        [HttpGet]
        public async Task<IActionResult> GetUserVendorAsync([FromQuery] int userStatus)
            => CreateActionResult(await _userService.GetUserVendorAsync(User.Identity.Name, userStatus));

        [HttpPost]
        public async Task<IActionResult> UserChangeStatusAsync(List<UserChangeStatusModel> model)
          => CreateActionResult(await _userService.UserChangeStatusAsync(User.Identity.Name, model));

        [HttpPost]
        public async Task<IActionResult> SaveUserAsync([FromForm] UserSaveModel userSaveModel, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByEmailAsync(userSaveModel.Email);
            if (user is not null && (userSaveModel.Id == null || userSaveModel.Id == 0))
                return CreateActionResult(ApiResponse<bool>.Fail("email", $" This mail is already in use", 422));

            userSaveModel.VerifyToken = Helper.GetVerifyToken(_tokenHandler.CreateRefreshToken());

            var result = await _userService.SaveUserAsync(userSaveModel, cancellationToken);

            if (result.StatusCode == 200)
            {
                if (userSaveModel.Id == 0)
                {
                    var templateDataForVerification =
                           _emailNotificationService.GetEmailTemplateData(Language.az, EmailTemplateKey.VER).Result;
                    var companyName = _emailNotificationService.GetCompanyName(userSaveModel.Email).Result;

                    VM_EmailVerification emailVerification = new VM_EmailVerification
                    {
                        Username = userSaveModel.UserName,
                        Body = new HtmlString(string.Format(templateDataForVerification.Body, userSaveModel.FullName)),
                        CompanyName = companyName,
                        Header = templateDataForVerification.Header,
                        Language = Language.az,
                        Subject = templateDataForVerification.Subject,
                        Token = HttpUtility.HtmlDecode(userSaveModel.VerifyToken),
                    };

                    Response.OnCompleted(async () =>
                    {
                        await _mailService.SendUsingTemplate(templateDataForVerification.Subject, emailVerification,
                            emailVerification.TemplateName(), emailVerification.ImageName(),
                            new List<string> { userSaveModel.Email });
                    });
                }
            }

            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPasswordAsync(ChangeUserPasswordModel user)
            => CreateActionResult(await _userService.ChangeUserPasswordAsync(user));

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfoAsync(int userId)
        {
            string token = _tokenHandler.GetAccessToken();
            return CreateActionResult(await _userService.GetUserInfoAsync(userId, token));
        }

        [HttpGet]
        public async Task<IActionResult> GetERPUserAsync()
            => CreateActionResult(await _userService.GetERPUserAsync());

        [HttpPost]
        public async Task<IActionResult> UserSendToApprove()
          => CreateActionResult(await _userService.UserSendToApprove(5714.ToString()));

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(DeleteUser deleteUser)
            => CreateActionResult(await _userService.DeleteUserAsync(deleteUser));

        [HttpGet]
        public async Task<IActionResult> GetUsersByGroupIdAsync(int groupId)
            => CreateActionResult(await _userService.GetUsersByGroupIdAsync(groupId));

        [HttpPost]
        public async Task<IActionResult> AddGroupToUserAsync(AddUserToGroupModel addUserToGroupModel)
            => CreateActionResult(await _userService.AddGroupToUserAsync(addUserToGroupModel.groupIds, addUserToGroupModel.UserId));

        [HttpPost]
        public async Task<IActionResult> DeleteGroupFromUserAsync(AddUserToGroupModel addUserToGroupModel)
          => CreateActionResult(await _userService.DeleteGroupFromUserAsync(addUserToGroupModel.groupIds, addUserToGroupModel.UserId));

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserInfoAsync()
        {
            string token = _tokenHandler.GetAccessToken();
            return CreateActionResult(await _userService.GetUserByNameAsync(User.Identity.Name, token));
        }


    }
}
