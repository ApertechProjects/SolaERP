using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Models;
using SolaERP.Business.Dtos.EntityDtos.User;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : CustomBaseController
    {
        private readonly UserManager<Application.Entities.Auth.User> _userManager;
        private readonly SignInManager<Application.Entities.Auth.User> _signInManager;
        private readonly IUserService _userService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Application.Entities.Auth.User> userManager,
                                 SignInManager<Application.Entities.Auth.User> signInManager,
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
                var token = await _tokenHandler.GenerateJwtTokenAsync(30, userdto);
                await _userService.UpdateUserIdentifierAsync(user.Id, token.RefreshToken, token.Expiration, 5);

                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = token }, 200));
            }


            return CreateActionResult(ApiResponse<bool>.Fail("email", "Email or password is incorrect", 422));
        }

        //[HttpPost]
        //public async Task<IActionResult> LoginRefreshTokenAsync(string refreshToken)
        //    => CreateActionResult(await _userService.RefreshTokenLoginAsync(refreshToken));

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel dto)
        {
            var newtoken = Guid.NewGuid();
            //dto.UserToken = newtoken + _tokenHandler.CreateRefreshToken();
            ApiResponse<bool> response = response = await _userService.UserRegisterAsync(dto);

            AccountResponseDto account = new();
            if (response.Data)
            {
                account.Token = await _tokenHandler.GenerateJwtTokenAsync(60, dto);
                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(account, 200));
            }

            return CreateActionResult(ApiResponse<bool>.Fail(response.Errors, 422));
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
