using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Services;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Services;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        public AccountController(UserService userService,
                                 SignInManager<User> signInManager,
                                 UserManager<User> userManager,
                                 ITokenHandler handler)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = handler;
        }


        [HttpGet]
        public ApiResponse<List<UserDto>> GetAllUsers()
        {
            return _userService.GetAll();
        }

        [HttpPost]
        public async Task<ApiResponse<Token>> Login(LoginRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            //
            if (user == null)
                return ApiResponse<Token>.Fail("User not found", 404);

            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, true, false);
            if (signInResult.Succeeded)
                return ApiResponse<Token>.Success(await _tokenHandler.GenerateJwtTokenAsync(2), 200);

            return ApiResponse<Token>.Fail("User cant sign in", 403);
        }

        [HttpPost]
        public async Task<ApiResponse<Token>> Register(UserDto dto)
        {
            return await _userService.AddAsync(dto);
        }

        [HttpPut]
        public async Task<ApiResponse<bool>> UpdateUser(UserDto dto)
        {
            return await _userService.UpdateUser(dto);
        }

        [HttpDelete]
        public ApiResponse<bool> RemoveUser(UserDto dto)
        {
            return _userService.RemoveUser(dto);
        }
    }
}
