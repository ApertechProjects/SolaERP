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
        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           IMailService mailService,
                           IFileService fileService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
            _fileService = fileService;
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

        public async Task<ApiResponse<bool>> SendResetPasswordEmail(string email)
        {
            var userExsist = await _userRepository.GetUserByEmailAsync(email);

            Random random = new Random();
            var stringCode = random.Next(0, 999999).ToString();

            if (userExsist == null)
                return ApiResponse<bool>.Fail("email", $"We can't found this email: {email}", 422);

            await _userRepository.SetUserEmailCode(stringCode, userExsist.Id);

            await _mailService.SendPasswordResetMailAsync(email, stringCode);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<UserDto>> GetUserByTokenAsync(string finderToken)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null)
                return ApiResponse<UserDto>.Fail("user", "User not found", 422, true);

            var dto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<bool>> RemoveUserByTokenAsync(string finderToken)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(finderToken);

            if (userId == 0)
                return ApiResponse<bool>.Fail("email", "User not found", 422, true);

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

        public async Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersWithoutCurrentUserAsync(string finderToken)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(finderToken);
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

        public Task<string> GetUserNameByTokenAsync(string finderToken)
        {
            return _userRepository.GetUserNameByTokenAsync(finderToken);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserWFAAsync(string authToken, UserGetModel model)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var users = await _userRepository.GetUserWFAAsync(userId, model);
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
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
            return ApiResponse<List<UserMainDto>>.Fail("user", "List is empty", 404, true);

        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserAllAsync(string authToken, UserGetModel model)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            var users = await _userRepository.GetUserAllAsync(userId, model);
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
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
            return ApiResponse<List<UserMainDto>>.Fail("user", "List is empty", 404, true);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserCompanyAsync(string authToken, int userStatus)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
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
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
            return ApiResponse<List<UserMainDto>>.Fail("user", "List is empty", 404, true);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserVendorAsync(string authToken, int userStatus)
        {
            int userId = await _userRepository.GetUserIdByTokenAsync(authToken);
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
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
            return ApiResponse<List<UserMainDto>>.Fail("user", "List is empty", 404, true);
        }

        public async Task<ApiResponse<bool>> UserChangeStatusAsync(string authToken, UserChangeStatusModel model)
        {
            var userId = await _userRepository.GetUserIdByTokenAsync(authToken);
            if (model.Id == 0)
                return ApiResponse<bool>.Fail("user", "User must be selected", 422);

            List<string> failedMailList = new List<string>();
            string userName = await _userRepository.GetUserNameByTokenAsync(authToken);

            var user = await _userRepository.UserChangeStatusAsync(userId, model);

            await _unitOfWork.SaveChangesAsync();
            if (user)
                return ApiResponse<bool>.Success(true, 200);
            return ApiResponse<bool>.Fail("user", "Problem detected", 422);
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
                return ApiResponse<bool>.Fail("Confirm Password", " Confirm Password doesn't match the Password!", 422);

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

            return ApiResponse<bool>.Success(result ? true : false, 200);
        }

        public async Task<bool> CheckTokenAsync(string authToken)
        {
            var check = await _userRepository.CheckTokenAsync(authToken);
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
                : ApiResponse<bool>.Fail("internal server error something went wrong", 500);
        }

        public Task<ApiResponse<List<UsersByGroupDto>>> GetUsersByGroupIdAsync(int groupId)
        {
            throw new NotImplementedException();
        }
    }
}
