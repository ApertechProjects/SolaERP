using AutoMapper;
using SolaERP.Application.Exceptions;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Repositories;
using SolaERP.Infrastructure.Services;
using SolaERP.Infrastructure.UnitOfWork;
using System.Net.Sockets;
using System.Net;
using SolaERP.Infrastructure.Dtos.User;
using System.Reflection;

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

        public async Task<UserDto> AddAsync(UserDto model)
        {
            if (model.PasswordHash != model.ConfirmPasswordHash)
                throw new UserException("Password doesn't match with confirm password");

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
                UserDto userDto = _mapper.Map<UserDto>(test);
                Kernel.CurrentUserId = test.Id;
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
        public async Task<ApiResponse<bool>> UpdateAsync(UserDto model)
        {
            return ApiResponse<bool>.Success(200);
        }
        public async Task<ApiResponse<bool>> RemoveAsync(UserDto model)
        {
            var user = _mapper.Map<User>(model);
            _userRepository.Remove(user);

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
