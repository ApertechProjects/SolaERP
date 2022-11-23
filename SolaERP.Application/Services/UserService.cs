using AutoMapper;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.UserDto;
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
        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           ITokenHandler tokenHandler)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
        }

        public async Task<ApiResponse<bool>> AddAsync(UserDto model)
        {
            if (model.PasswordHash != model.ConfirmPasswordHash)
                throw new InvalidOperationException("Password doesn't match with confirm password");

            var userExist = await _userRepository.GetByUserNameAsync(model.UserName);

            model.PasswordHash = SecurityUtil.ComputeSha256Hash(model.PasswordHash);
            var user = _mapper.Map<User>(model);

            Guid guid = Guid.NewGuid();
            user.UserToken = guid;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;

            var result = await _userRepository.AddAsync(user);
            if (result)
            {
                User test = await _userRepository.GetLastInsertedUserAsync();
                Kernel.CurrentUserId = test.Id;
                return ApiResponse<Token>.Success(await _tokenHandler.GenerateJwtTokenAsync(test, 2), 200);
            }
            return ApiResponse<Token>.Fail("User Cant added", 400);
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);

            return ApiResponse<List<UserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(UserDto model)
        {
            User user = await _userRepository.GetByUserNameAsync(model.UserName);

            var result = _mapper.Map<User>(user);
            _userRepository.Update(result);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<bool>> RemoveAsync(UserDto model)
        {
            var user = _mapper.Map<User>(model);
            _userRepository.Remove(user);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

    }
}
