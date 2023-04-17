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
                return CreateActionResult(ApiResponse<bool>.Fail("email", $" {dto.Email} not found", 422));

            var userdto = _mapper.Map<UserRegisterModel>(user);
            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (signInResult.Succeeded)
            {
                var newtoken = Guid.NewGuid();
                var checkToken = CheckTokensRecursively(newtoken);

                await _userService.UpdateUserIdentifierAsync(user.Id, await checkToken);

                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(1, userdto), UserIdentifier = newtoken.ToString() }, 200));
            }


            return CreateActionResult(ApiResponse<bool>.Fail("email", "Email or password is incorrect", 422));
        }

        async Task<Guid> CheckTokensRecursively(Guid currentToken)
        {
            bool isValid = await _userService.CheckTokenAsync(currentToken);

            if (isValid)
            {
                // If the token is valid, recursively call this method again with the new token.
                var newToken = Guid.NewGuid();
                return await CheckTokensRecursively(newToken);
            }
            else
            {
                // If the token is not valid, return it.
                return currentToken;
            }
        }
        /// <summary>
        /// Creates a user with given input
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);

            if (user is null)
            {
                var newtoken = Guid.NewGuid();
                var checkToken = CheckTokensRecursively(newtoken);
                dto.UserToken = await checkToken;
                await _userService.UserRegisterAsync(dto);

                return CreateActionResult(ApiResponse<AccountResponseDto>.Success(new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, dto), UserIdentifier = dto.UserToken.ToString() }, 200));
            }
            return CreateActionResult(ApiResponse<bool>.Fail("email", "This email is already exsist", 422));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            var userId = await _userService.GetIdentityNameAsIntAsync(User.Identity.Name);
            await _userService.UpdateUserIdentifierAsync(userId, new Guid());

            return CreateActionResult(ApiResponse<bool>.Success(true, 200));
        }

        /// <summary>
        /// Allows users to reset their password if they forget it.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel resetPasswordrequestDto)
            => CreateActionResult(await _userService.ResetPasswordAsync(resetPasswordrequestDto));

    }
}
