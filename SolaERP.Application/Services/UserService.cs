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
using System.Collections.Generic;
using System.Reflection;

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
            if (model.UserType == Infrastructure.Enums.UserRegisterType.SupplierUser && model.VendorId == 0)
                throw new UserException("Company name required for Supplier user");

            if (model.Password != model.ConfirmPassword)
                throw new UserException("Password doesn't match with confirm password");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(model.Password);

            var result = await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UserRegisterAsync(UserRegisterModel model)
        {
            if (model.UserType == Infrastructure.Enums.UserRegisterType.SupplierUser && model.VendorId == 0)
                throw new UserException("Company name required for Supplier user");

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

        public async Task<int> GetUserIdByTokenAsync(string finderToken)
        {
            return await _userRepository.GetUserIdByTokenAsync(finderToken);
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
        } //GetActiveUsersWithoutCurrenctUserAsync

        public async Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersWithoutCurrentUserAsync(string finderToken)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var users = await _userRepository.GetActiveUsersWithoutCurrentUserAsync(userId);
            var dto = _mapper.Map<List<ActiveUserDto>>(users);

            return ApiResponse<List<ActiveUserDto>>.Success(dto, 200);
        } //GetAct

        public async Task<ApiResponse<List<UserListDto>>> GetUserListAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserListDto>>(users);

            return ApiResponse<List<UserListDto>>.Success(dto, 200);
        }

        public Task<string> GetUserNameByTokenAsync(string finderToken)
        {
            return _userRepository.GetUserNameByTokenAsync(finderToken);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserWFAAsync(string authToken, UserGetModel model)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var users = await _userRepository.GetUserWFAAsync(userId, model);
            var dto = _mapper.Map<List<UserMainDto>>(users);
            return ApiResponse<List<UserMainDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserAllAsync(string authToken, UserGetModel model)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var users = await _userRepository.GetUserAllAsync(userId, model);
            var dto = _mapper.Map<List<UserMainDto>>(users);
            return ApiResponse<List<UserMainDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserCompanyAsync(string authToken, List<int> userStatus, bool all)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var users = await _userRepository.GetUserCompanyAsync(userId, userStatus, all);
            var dto = _mapper.Map<List<UserMainDto>>(users);
            return ApiResponse<List<UserMainDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserVendorAsync(string authToken, List<int> userStatus, bool all)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var users = await _userRepository.GetUserVendorAsync(userId, userStatus, all);
            var dto = _mapper.Map<List<UserMainDto>>(users);
            return ApiResponse<List<UserMainDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> UserChangeStatusAsync(string authToken, UserChangeStatusModel model)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            if (model.Id == 0)
                return ApiResponse<bool>.Fail("User must be selected", 200);

            List<string> failedMailList = new List<string>();
            string userName = await _userRepository.GetUserNameByTokenAsync(authToken);

            await _userRepository.UserChangeStatusAsync(userId, model);

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<UserLoadDto>> GetUserInfo(int userId)
        {
            var user = await _userRepository.GetUserInfoAsync(userId);
            var dto = _mapper.Map<UserLoadDto>(user);
            return ApiResponse<UserLoadDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ERPUserDto>>> GetERPUser()
        {
            var user = await _userRepository.GetERPUser();
            var dto = _mapper.Map<List<ERPUserDto>>(user);
            return ApiResponse<List<ERPUserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel)
        {
            if (passwordModel.Password != passwordModel.ConfirmPassword)
                return ApiResponse<bool>.Fail("Password and Confirm Password must be equalent", 422);

            passwordModel.Password = SecurityUtil.ComputeSha256Hash(passwordModel.Password);
            var user = await _userRepository.ChangeUserPasswordAsync(passwordModel);
            await _unitOfWork.SaveChangesAsync();
            if (user)
                return ApiResponse<bool>.Success(user, 200);
            return ApiResponse<bool>.Fail("User must be selected", 200);
        }
    }
}
