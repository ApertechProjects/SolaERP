using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Models;

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


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
                return CreateActionResult(ApiResponse<AccountResponseDto>.Fail($"User: {dto.Email} not found", 400));

            var userdto = _mapper.Map<UserDto>(user);
            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (signInResult.Succeeded)
            {
                var newtoken = Guid.NewGuid();
                await _userService.UpdateUserIdentifierAsync(user.UserToken.ToString(), newtoken);

                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, userdto), UserIdentifier = newtoken.ToString() }, 200));
            }
            return CreateActionResult(ApiResponse<AccountResponseDto>.Fail("Email or password is incorrect", 400));
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);

            if (user is null)
            {
                dto.UserToken = Guid.NewGuid();
                await _userService.AddAsync(dto);

                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, dto), UserIdentifier = dto.UserToken.ToString() }, 200));
            }
            return CreateActionResult(ApiResponse<AccountResponseDto>.Fail("This email is already exsist", 400));
        }


        [HttpPost]
        public async Task<IActionResult> Logout([FromHeader] string authToken)
        {
            await _signInManager.SignOutAsync();

            var userId = await _userService.GetUserIdByTokenAsync(authToken);
            await _userService.UpdateUserIdentifierAsync(authToken, new Guid());

            return CreateActionResult(ApiResponse<bool>.Success(true, 200));
        }



        [HttpGet("{email}")]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
            => CreateActionResult(await _userService.SendResetPasswordEmail(email, @"C:\Users\HP\source\repos\SolaERP\SolaERP\Templates\EmailTemplate.html"));


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordrequestDto)
            => CreateActionResult(await _userService.ResetPasswordAsync(resetPasswordrequestDto));
    }
}
