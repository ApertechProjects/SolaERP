using AutoMapper;
using SolaERP.Application.Exceptions;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> AddAsync(UserDto model)
        {
            if (model.Password != model.ConfirmPassword)
                throw new UserException("Password doesn't match with confirm password");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(model.Password);

            var result = await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            if (result)
            {
                User lastInsertedUser = await _userRepository.GetLastInsertedUserAsync();
                UserDto userDto = _mapper.Map<UserDto>(lastInsertedUser);
                Kernel.CurrentUserId = lastInsertedUser.Id;
                return userDto;
            }
            return null;
        }
        public async Task<ApiResponse<List<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);

            return ApiResponse<List<UserDto>>.Success(dto, 200);
        }
        public async Task<ApiResponse<bool>> UpdateAsync(UserDto userUpdateDto)
        {
            if (userUpdateDto.Password != userUpdateDto.ConfirmPassword)
                throw new UserException("Password doesn't match with confirm password");

            var user = _mapper.Map<User>(userUpdateDto);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(userUpdateDto.Password);
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }
        public async Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            _userRepository.Remove(Id);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }
        public async Task<UserDto> GetByUserId(int userId)
        {
            var userDatas = await _userRepository.GetByUserId(userId);
            var userDto = _mapper.Map<UserDto>(userDatas);
            return userDto;
        }
        public async Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var result = _mapper.Map<User>(userUpdateDto);
            _userRepository.Update(result);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }
        public async Task<ApiResponse<bool>> UpdateUserPassword(UserUpdatePasswordDto userUpdatePasswordDto)
        {

            //if (userUpdatePasswordDto.PasswordHash != userUpdatePasswordDto.ConfirmPasswordHash)
            //    throw new UserException("Password doesn't match with confirm password");

            //var result = _mapper.Map<User>(userUpdatePasswordDto);
            //_userRepository.Update
            return ApiResponse<bool>.Success(200);
        }



    }
}
