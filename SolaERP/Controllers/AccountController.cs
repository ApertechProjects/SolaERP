using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Models;
using System.Text.RegularExpressions;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : CustomBaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IUserService userService,
                                 ITokenHandler handler,
                                 IMapper mapper)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = handler;
            _mapper = mapper;
        }


        /// <summary>
        /// Sends Reset Password message template to given email.Then redirects to ResetPassword
        /// </summary>
        [HttpGet("{email}")]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
            => CreateActionResult(await _userService.SendResetPasswordEmail(email));

        /// <summary>
        /// Logs user into system and returns token
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
                return CreateActionResult(ApiResponse<bool>.Fail("user", $" {dto.Email} not found", 422));

            var userdto = _mapper.Map<UserRegisterModel>(user);
            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (signInResult.Succeeded)
            {
                var newtoken = Guid.NewGuid();
                await _userService.UpdateUserIdentifierAsync(user.UserToken.ToString(), newtoken);

                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(1, userdto), UserIdentifier = newtoken.ToString() }, 200));
            }


            return CreateActionResult(ApiResponse<bool>.Fail("email", "Email or password is incorrect", 422));
        }

        /// <summary>
        /// Creates a user with given input
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel dto)
        {
            //bool isValidUserName = RegexEmailCheck(dto.UserName);
            //if (!isValidUserName)
            //    return CreateActionResult(ApiResponse<bool>.Fail("userName", "Please, enter valid User Name", 422));

            //bool isValidEmail = RegexEmailCheck(dto.Email);
            //if (!isValidEmail)
            //    return CreateActionResult(ApiResponse<bool>.Fail("email", "Please, enter valid Email", 422));

            //bool isValidFullName = RegexEmailCheck(dto.FullName);
            //if (!isValidFullName)
            //    return CreateActionResult(ApiResponse<bool>.Fail("fullName", "Please, enter valid Full Name", 422));

            var user = await _userManager.FindByNameAsync(dto.Email);

            if (user is null)
            {
                dto.UserToken = Guid.NewGuid();
                await _userService.UserRegisterAsync(dto);

                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, dto), UserIdentifier = dto.UserToken.ToString() }, 200));
            }
            return CreateActionResult(ApiResponse<bool>.Fail("email", "This email is already exsist", 422));
        }

        public static bool RegexEmailCheck(string input)
        {
            // returns true if the input is a valid email
            return Regex.IsMatch(input, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }


        [HttpPost]
        public async Task<IActionResult> Logout([FromHeader] string authToken)
        {
            await _signInManager.SignOutAsync();

            //var userId = await _userService.GetUserIdByTokenAsync(authToken);
            await _userService.UpdateUserIdentifierAsync(authToken, new Guid());

            return CreateActionResult(ApiResponse<bool>.Success(true, 200));
        }

        /// <summary>
        /// Allows users to reset their password if they forget it.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel resetPasswordrequestDto)
            => CreateActionResult(await _userService.ResetPasswordAsync(resetPasswordrequestDto));

        [HttpPost]
        public async Task<IActionResult> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel)
            => CreateActionResult(await _userService.ChangeUserPasswordAsync(passwordModel));


    }
}
