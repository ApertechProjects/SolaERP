using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           ITokenHandler tokenHandler,
                           UserManager<User> userManager,
                           SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ApiResponse<bool> Register(UserDto model)
        {
            model.PasswordHash = Utils.SecurityUtil.ComputeSha256Hash(model.PasswordHash);
            var user = _mapper.Map<User>(model);
            System.Guid guid = System.Guid.NewGuid();
            user.UserToken = guid;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            var result = _userRepository.Add(user);

            _unitOfWork.SaveChanges();
            return ApiResponse<bool>.Success(200);
        }

        public ApiResponse<List<UserDto>> GetAll()
        {
            var users = _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);

            return ApiResponse<List<UserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> UpdateUser(UserDto model)
        {
            User user = await _userRepository.GetByUserNameAsync(model.UserName);

            var result = _mapper.Map<User>(user);
            _userRepository.Update(result);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public ApiResponse<bool> RemoveUser(UserDto model)
        {
            var user = _mapper.Map<User>(model);
            _userRepository.Remove(user);

            _unitOfWork.SaveChanges();
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<Token>> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Email);
            if (user == null) return ApiResponse<Token>.Fail("User not found", 404);

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, true, false);

            if (signInResult.Succeeded)
                return ApiResponse<Token>.Success(await _tokenHandler.GenerateJwtTokenAsync(2), 200);

            return ApiResponse<Token>.Fail("User Cant Signed in", 403);
        }
    }
}
