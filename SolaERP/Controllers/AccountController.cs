using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;

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
        public async Task<ApiResponse<AccountResponseDto>> Login(LoginRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
                return ApiResponse<AccountResponseDto>.Fail($"User: {dto.Email} not found", 400);

            var userdto = _mapper.Map<UserDto>(user);
            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (signInResult.Succeeded)
            {
                Kernel.CurrentUserId = user.Id;
                return ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, userdto), AccountUser = userdto }, 200);
            }

            return ApiResponse<AccountResponseDto>.Fail("Email or password is incorrect", 400);
        }

        [HttpPost]
        public async Task<ApiResponse<AccountResponseDto>> Register(UserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                var result = await _userService.AddAsync(dto);

                if (result != null)
                    return ApiResponse<AccountResponseDto>.Success(
                        new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, result), AccountUser = result }, 200);
            }
            return ApiResponse<AccountResponseDto>.Fail("This email is already exsist", 400);
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> Logout()
        {
            await _signInManager.SignOutAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

    }
}
