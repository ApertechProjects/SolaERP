using AutoMapper;
using SolaERP.Application.Utils;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Group;
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
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;

        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           IMailService mailService,
                           IFileService fileService,
                           ITokenHandler tokenHandler)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
            _fileService = fileService;
            _tokenHandler = tokenHandler;
        }

        public async Task AddAsync(UserDto model)
        {
            if (model.UserType == Infrastructure.Enums.UserRegisterType.SupplierUser && model.VendorId == 0)
                ApiResponse<bool>.Fail("companyName", "Company name required for Supplier user", 422);

            if (model.Password != model.ConfirmPassword)
                ApiResponse<bool>.Fail("password", "Password doesn't match with confirm password", 422);


            var user = _mapper.Map<User>(model);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(model.Password);

            var result = await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UserRegisterAsync(UserRegisterModel model)
        {
            if (model.UserType == Infrastructure.Enums.UserRegisterType.SupplierUser && model.VendorId == 0)
                ApiResponse<bool>.Fail("companyName", "Company name required for Supplier user", 422);

            if (model.Password != model.ConfirmPassword)
                ApiResponse<bool>.Fail("password", "Password doesn't match with confirm password", 422);

            var user = _mapper.Map<User>(model);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(model.Password);

            var result = await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);
            if (dto.Count == 0)
                return ApiResponse<List<UserDto>>.Fail("User list is empty", 404);
            return ApiResponse<List<UserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(UserDto userUpdateDto)
        {
            if (userUpdateDto.Password != userUpdateDto.ConfirmPassword)
                ApiResponse<bool>.Fail("password", "Password doesn't match with confirm password", 422);
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
            var user = await _userRepository.GetUserByEmailCode(resetPasswordRequestDto.ResetPasswordCode);

            if (user == null)
                return ApiResponse<bool>.Fail("resetPasswordCode", $"You entered wrong Code: ", 422);


            if (resetPasswordRequestDto.Password == resetPasswordRequestDto.ConfirmPassword)
            {
                user.PasswordHash = SecurityUtil.ComputeSha256Hash(resetPasswordRequestDto.Password);
                await _userRepository.ResetUserPasswordAsync(user.Email, user.PasswordHash);


                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(true, 200);
            }

            return ApiResponse<bool>.Fail("password", "Password does not match with ConfirmPassword", 422);
        }

        public async Task<ApiResponse<NoContentDto>> UpdateUserIdentifierAsync(int userId, Guid newToken)
        {
            var isSuccessfull = await _userRepository.UpdateUserTokenAsync(userId, newToken);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<NoContentDto>.Success(200);
        }

        public async Task<int> GetIdentityNameAsIntAsync(string name)
        {
            return await _userRepository.GetIdentityNameAsIntAsync(name);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<ApiResponse<bool>> SendResetPasswordEmail(string email)
        {
            var userExsist = await _userRepository.GetUserByEmailAsync(email);

            Random random = new Random();
            var stringCode = random.Next(0, 999999).ToString();

            if (userExsist == null)
                return ApiResponse<bool>.Fail($"We can't found this email: {email}", 404);

            await _userRepository.SetUserEmailCode(stringCode, userExsist.Id);

            await _mailService.SendPasswordResetMailAsync(email, stringCode);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<UserDto>> GetUserByTokenAsync(string name)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null)
                return ApiResponse<UserDto>.Fail("User not found", 404);

            var dto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> RemoveUserByTokenAsync(string name)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);

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

        public async Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersWithoutCurrentUserAsync(string name)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var users = await _userRepository.GetActiveUsersWithoutCurrentUserAsync(userId);
            var dto = _mapper.Map<List<ActiveUserDto>>(users);

            return ApiResponse<List<ActiveUserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<UserListDto>>> GetUserListAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserListDto>>(users);

            return ApiResponse<List<UserListDto>>.Success(dto, 200);
        }

        public Task<string> GetUserNameByTokenAsync(string name)
        {
            return _userRepository.GetUserNameByTokenAsync(name);
        }

        public async Task<ApiResponse<(int,List<UserMainDto>)>> GetUserWFAAsync(string name, int userStatus, int userType)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var users = await _userRepository.GetUserWFAAsync(userId, userStatus, userType);
            var dto = _mapper.Map<List<UserMainDto>>(users.Item2);

            for (int i = 0; i < dto.Count; i++)
            {
                var user = dto.FirstOrDefault(x => x.UserName == users.Item2[i].UserName);
                if (!string.IsNullOrEmpty(users.Item2[i].UserPhoto))
                {
                    user.Photo = await _fileService.DownloadPhotoWithNetworkAsBase64Async(users.Item2[i].UserPhoto);
                }
            }

            if (dto.Count > 0)
                return ApiResponse<(int,List<UserMainDto>)>.Success((users.Item1,dto), 200, dto.Count);
            return ApiResponse<(int,List<UserMainDto>)>.Fail("User list is empty", 404);

        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserAllAsync(string name, int userStatus, int userType)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var users = await _userRepository.GetUserAllAsync(userId, userStatus, userType);
            var dto = _mapper.Map<List<UserMainDto>>(users);

            for (int i = 0; i < dto.Count; i++)
            {
                var user = dto.FirstOrDefault(x => x.UserName == users[i].UserName);
                if (!string.IsNullOrEmpty(users[i].UserPhoto))
                {
                    user.Photo = await _fileService.DownloadPhotoWithNetworkAsBase64Async(users[i].UserPhoto);
                }
            }

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200, dto.Count);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserCompanyAsync(string name, int userStatus)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var users = await _userRepository.GetUserCompanyAsync(userId, userStatus);
            var dto = _mapper.Map<List<UserMainDto>>(users);

            for (int i = 0; i < dto.Count; i++)
            {
                var user = dto.FirstOrDefault(x => x.UserName == users[i].UserName);
                if (!string.IsNullOrEmpty(users[i].UserPhoto))
                {
                    user.Photo = await _fileService.DownloadPhotoWithNetworkAsBase64Async(users[i].UserPhoto);
                }
            }

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200, dto.Count);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserVendorAsync(string name, int userStatus)
        {
            int userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            var users = await _userRepository.GetUserVendorAsync(userId, userStatus);
            var dto = _mapper.Map<List<UserMainDto>>(users);

            for (int i = 0; i < dto.Count; i++)
            {
                var user = dto.FirstOrDefault(x => x.UserName == users[i].UserName);
                if (!string.IsNullOrEmpty(users[i].UserPhoto))
                {
                    user.Photo = await _fileService.DownloadPhotoWithNetworkAsBase64Async(users[i].UserPhoto);
                }
            }

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200, dto.Count);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<bool>> UserChangeStatusAsync(string name, UserChangeStatusModel model)
        {
            var userId = await _userRepository.GetIdentityNameAsIntAsync(name);
            if (model.Id == 0)
                return ApiResponse<bool>.Fail("User not found", 404);

            List<string> failedMailList = new List<string>();

            var user = await _userRepository.UserChangeStatusAsync(userId, model);

            await _unitOfWork.SaveChangesAsync();
            if (user)
                return ApiResponse<bool>.Success(true, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<UserLoadDto>> GetUserInfo(int userId)
        {
            var user = await _userRepository.GetUserInfoAsync(userId);
            var dto = _mapper.Map<UserLoadDto>(user);
            if (!string.IsNullOrEmpty(user.UserPhoto))
                user.UserPhoto = await _fileService.DownloadPhotoWithNetworkAsBase64Async(user.UserPhoto);

            if (!string.IsNullOrEmpty(user.SignaturePhoto))
                user.SignaturePhoto = await _fileService.DownloadPhotoWithNetworkAsBase64Async(user.SignaturePhoto);

            return ApiResponse<UserLoadDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ERPUserDto>>> GetERPUserAsync()
        {
            var user = await _userRepository.GetERPUser();
            var dto = _mapper.Map<List<ERPUserDto>>(user);
            return ApiResponse<List<ERPUserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> SaveUserAsync(UserSaveModel user)
        {
            var userEntry = _mapper.Map<User>(user);
            string serverFilePath = string.Empty;
            string serverSignaturePath = string.Empty;

            if (user.Password != user.ConfirmPassword)
                return ApiResponse<bool>.Fail("confirm Password", " Confirm Password doesn't match the Password!", 422);

            if (!string.IsNullOrEmpty(user.Password))
                userEntry.PasswordHash = SecurityUtil.ComputeSha256Hash(user?.Password);

            if (!string.IsNullOrEmpty(user.Files?.Base64Photo))
                serverFilePath = await _fileService.UploadBase64PhotoWithNetworkAsync(user.Files.Base64Photo, user.Files.Extension, user.Files.PhotoFileName);

            if (!string.IsNullOrEmpty(user.Files?.Base64Signature))
                serverSignaturePath = await _fileService.UploadBase64PhotoWithNetworkAsync(user.Files.Base64Signature, user.Files.Extension, user.Email + "signature");

            userEntry.SignaturePhoto = serverSignaturePath;
            userEntry.UserPhoto = serverFilePath;

            var result = await _userRepository.SaveUserAsync(userEntry);
            await _unitOfWork.SaveChangesAsync();

            if (result)
                return ApiResponse<bool>.Success(result, 200);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public async Task<bool> CheckTokenAsync(Guid name)
        {
            var check = await _userRepository.CheckTokenAsync(name);
            if (check) return true;
            else return false;
        }

        public async Task<ApiResponse<bool>> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel)
        {
            if (passwordModel.Password != passwordModel.ConfirmPassword)
                return ApiResponse<bool>.Fail("password", "Password must be equal to Confirm password", 422);
            passwordModel.Password = SecurityUtil.ComputeSha256Hash(passwordModel.Password);
            var pass = await _userRepository.ChangeUserPasswordAsync(passwordModel);
            if (pass) return ApiResponse<bool>.Success(200);
            else return ApiResponse<bool>.Success(400);
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(List<int> userIds)
        {
            int succesfulCounter = 0;
            List<Task<bool>> tasks = new List<Task<bool>>();

            userIds.ForEach(x =>
            {
                tasks.Add(_userRepository.SaveUserAsync(new() { Id = x }));
            });

            bool[] results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                if (result) succesfulCounter++;
            }

            await _unitOfWork.SaveChangesAsync();

            return succesfulCounter == userIds.Count
                ? ApiResponse<bool>.Success(204)
                : ApiResponse<bool>.Fail("User can not be deleted", 400);
        }

        public async Task<ApiResponse<List<UsersByGroupDto>>> GetUsersByGroupIdAsync(int groupId)
        {
            var grupser = await _userRepository.GetUsersByGroupIdAsync(groupId);
            var dto = _mapper.Map<List<UsersByGroupDto>>(grupser);

            return dto.Capacity > 0 ? ApiResponse<List<UsersByGroupDto>>.Success(dto, 200)
                              : ApiResponse<List<UsersByGroupDto>>.Fail("No users found for the specified group .", 404);

        }

        public async Task<bool> UpdateSessionAsync(int userId, int updateCommand)
        {
            return await _userRepository.UpdateSessionAsync(userId, updateCommand);
        }
    }
}
