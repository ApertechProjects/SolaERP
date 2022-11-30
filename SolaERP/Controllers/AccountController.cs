using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IUserService userService,
                                 ITokenHandler handler,
                                 IMapper mapper,
                                 IEmailService emailService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = handler;
            _mapper = mapper;
            _emailService = emailService;
        }


        [HttpGet]
        [Authorize]
        public int GetCurrentUserId()
        {
            return Kernel.CurrentUserId;
        }

        [HttpGet]
        public async Task<ApiResponse<List<UserDto>>> GetAllUsers()
        {
            return await _userService.GetAllAsync();
        }

        [HttpPost]
        public async Task<ApiResponse<AccountResponseDto>> Login(LoginRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);

            if (user == null)
                return ApiResponse<AccountResponseDto>.Fail($"User: {dto.Email} not found", 400);

            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, true, false);

            if (signInResult.Succeeded)
            {
                Kernel.CurrentUserId = user.Id;
                return ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(2), AccountUser = _mapper.Map<UserDto>(user) }, 200);
            }

            return ApiResponse<AccountResponseDto>.Fail("Email or password is incorrect", 400);
        }

        [HttpPost]
        public async Task<ApiResponse<AccountResponseDto>> Register(UserDto dto)
        {
            var result = await _userService.AddAsync(dto);
            if (result != null)
                return ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(2), AccountUser = result }, 200);
            return ApiResponse<AccountResponseDto>.Fail("Email not found exception", 400);

        }

        [HttpGet]
        public async Task<UserDto> GetByUserId()
        {
            return await _userService.GetByUserId(Kernel.CurrentUserId);
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> UpdateUser(UserUpdateDto dto)
        {
            return await _userService.UpdateUserAsync(dto);
        }

        [HttpDelete]
        public async Task<ApiResponse<bool>> RemoveUser(UserDto dto)
        {
            return await _userService.RemoveAsync(dto);
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> Logout()
        {
            await _signInManager.SignOutAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

        [HttpPost]
        public ApiResponse<bool> SendEmailForResetPassword(UserResetPasswordDto dto)
        {
            return _emailService.SendEmailForResetPassword(dto);
        }

        [HttpPost]
        public ApiResponse<bool> VerifyIncomingCodeFromMail(string verifyCode)
        {
            return _emailService.VerifyIncomingCodeFromMail(verifyCode);
        }
    }
}
