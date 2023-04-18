using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.Shared;
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



        [HttpGet("{email}")]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
            => CreateActionResult(await _userService.SendResetPasswordEmail(email));


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
                return CreateActionResult(ApiResponse<bool>.Fail("email", $" {dto.Email} not found", 422));

            var userdto = _mapper.Map<UserRegisterModel>(user);
            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (signInResult.Succeeded)
            {
                await _userService.UpdateSessionAsync(user.Id, 1);
                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(1, userdto) }, 200));
            }


            return CreateActionResult(ApiResponse<bool>.Fail("email", "Email or password is incorrect", 422));
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);

            if (user is null)
            {
                await _userService.UserRegisterAsync(dto);
                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, dto) }, 200));
            }
            return CreateActionResult(ApiResponse<bool>.Fail("email", "This email is already exsist", 422));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            var userId = await _userService.GetIdentityNameAsIntAsync(User.Identity.Name);
            await _userService.UpdateSessionAsync(Convert.ToInt32(User.Identity.Name), -1);
            return CreateActionResult(ApiResponse<bool>.Success(true, 200));
        }


        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel resetPasswordrequestDto)
            => CreateActionResult(await _userService.ResetPasswordAsync(resetPasswordrequestDto));

    }
}
