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
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return ApiResponse<AccountResponseDto>.Fail($"User: {dto.Username} not found", 400);

            var userdto = _mapper.Map<UserDto>(user);
            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (signInResult.Succeeded)
            {
                var newtoken = Guid.NewGuid();
                await _userService.UpdateUserIdentifier(user.UserToken.ToString(), newtoken);

                return ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, userdto), UserIdentifier = newtoken.ToString() }, 200);
            }
            return ApiResponse<AccountResponseDto>.Fail("Email or password is incorrect", 400);
        }



        [HttpPost]
        public async Task<ApiResponse<AccountResponseDto>> Register(UserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                dto.UserToken = Guid.NewGuid();
                await _userService.AddAsync(dto);

                return ApiResponse<AccountResponseDto>.Success(
                    new AccountResponseDto { Token = await _tokenHandler.GenerateJwtTokenAsync(60, dto), UserIdentifier = dto.UserToken.ToString() }, 200);
            }
            return ApiResponse<AccountResponseDto>.Fail("This email is already exsist", 400);
        }



        [HttpPost("token")]
        public async Task<ApiResponse<bool>> Logout(string token)
        [HttpPost]
        public async Task<ApiResponse<bool>> ResetPassword(UserDto dto)
        {
            await _userService.UpdateAsync(dto);
            return ApiResponse<bool>.Success(200);
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> Logout()
        {
            await _signInManager.SignOutAsync();

            var userId = await _userService.GetUserIdByTokenAsync(token);
            await _userService.UpdateUserIdentifier(token, new Guid());

            return ApiResponse<bool>.Success(true, 200);
        }

    }
}
