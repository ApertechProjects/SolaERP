using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SolaERP.API.Methods;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Entities.Auth;
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


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfoAsync(int userId)
        {
            string token = _tokenHandler.GetAccessToken();
            return CreateActionResult(await _userService.GetUserInfoAsync(userId, token));
        }

     
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserInfoAsync()
        {
            return CreateActionResult(await _userService.GetCurrentUserInfo(User.Identity.Name));
        }


    }
}
