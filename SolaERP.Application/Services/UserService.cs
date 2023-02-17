using AutoMapper;
using SolaERP.Application.Exceptions;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;

namespace SolaERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           IMailService mailService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
        }

        public async Task AddAsync(UserDto model)
        {
            if (model.Password != model.ConfirmPassword)
                throw new UserException("Password doesn't match with confirm password");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(model.Password);

            var result = await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
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
            await _userRepository.UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<bool>> RemoveAsync(int Id)
        {
            await _userRepository.RemoveAsync(Id);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var userDatas = await _userRepository.GetUserByIdAsync(userId);
            var userDto = _mapper.Map<UserDto>(userDatas);
            return userDto;
        }

        public async Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var result = _mapper.Map<User>(userUpdateDto);
            await _userRepository.UpdateAsync(result);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordModel resetPasswordRequestDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(resetPasswordRequestDto.Email);

            if (user == null)
                return ApiResponse<bool>.Fail($"We can't find this email: {resetPasswordRequestDto.Email}", 404);

            if (resetPasswordRequestDto.Password == resetPasswordRequestDto.ConfirmPassword)
            {
                user.PasswordHash = SecurityUtil.ComputeSha256Hash(resetPasswordRequestDto.Password);
                await _userRepository.ResetUserPasswordAsync(resetPasswordRequestDto.Email, user.PasswordHash);

                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(true, 200);
            }

            return ApiResponse<bool>.Fail("Password does not match with ConfirmPassword", 400);
        }

        public async Task<ApiResponse<NoContentDto>> UpdateUserIdentifierAsync(string finderToken, Guid newToken)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);

            var isSuccessfull = await _userRepository.UpdateUserTokenAsync(userId, newToken);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<NoContentDto>.Success(200);
        }

        public Task<int> GetUserIdByTokenAsync(string finderToken)
        {
            return _userRepository.GetUserIdByTokenAsync(finderToken);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<ApiResponse<bool>> SendResetPasswordEmail(string email, string templatePath)
        {
            var userExsist = await _userRepository.GetUserByEmailAsync(email);

            if (userExsist == null)
                return ApiResponse<bool>.Fail($"We can't found this email: {email}", 404);

            await _mailService.SendPasswordResetMailAsync(email, templatePath);
            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<UserDto>> GetUserByTokenAsync(string finderToken)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null)
                return ApiResponse<UserDto>.Fail("User not found", 404);

            var dto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> RemoveUserByTokenAsync(string finderToken)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);

            if (userId == 0)
                return ApiResponse<bool>.Fail("User not found", 404);

            var result = _userRepository.RemoveAsync(userId);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(200);
        }

        public async Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetActiveUsersAsync();
            var dto = _mapper.Map<List<ActiveUserDto>>(users);

            return ApiResponse<List<ActiveUserDto>>.Success(dto, 200);
        }

    
    }
}
