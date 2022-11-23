using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Services;
using System.Security.Claims;

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
        private readonly IHttpContextAccessor _accessor;
        public AccountController(UserService userService,
                                 SignInManager<User> signInManager,
                                 UserManager<User> userManager,
                                 ITokenHandler handler,
                                 IHttpContextAccessor accessor)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = handler;
            _accessor = accessor;
        }


        [HttpGet]
        public async Task<ApiResponse<List<UserDto>>> GetAllUsers()
        {
            return await _userService.GetAllAsync();
        }

        [HttpPost]
        public async Task<ApiResponse<Token>> Login(LoginRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);

            if (user == null)
                return ApiResponse<Token>.Fail("User not found", 404);

            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, true, false);

            if (signInResult.Succeeded)
                return ApiResponse<Token>.Success(await _tokenHandler.GenerateJwtTokenAsync(1), 200);

            return ApiResponse<Token>.Fail("User cant sign in", 403);
        }

        [HttpPost]
        public async Task<ApiResponse<Token>> Register(UserDto dto)
        {
            var result = await _userService.AddAsync(dto);

            //return result ? ApiResponse<Token>.Success(_tokenHandler.GenerateJwtTokenAsync()):
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> UpdateUser(UserDto dto)
        {
            return await _userService.UpdateAsync(dto);
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
    }
}
