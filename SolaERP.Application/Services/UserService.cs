using AutoMapper;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Group;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Dtos.UserDto;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.Persistence.Utils;

namespace SolaERP.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        //private readonly IFileProducer _producer;

        public UserService(IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           IMailService mailService,
                           ITokenHandler tokenHandler)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            //_producer = producer;
        }


        public async Task<ApiResponse<int>> UserRegisterAsync(UserRegisterModel model)
        {
            var userExsist = await _userRepository.GetByEmailAsync(model.Email);

            if (userExsist is not null)
                return ApiResponse<int>.Fail("user", "This user is already exsist in our system", 422, false);

            if (model.UserType == Application.Enums.UserRegisterType.SupplierUser && model.VendorId == 0)
                return ApiResponse<int>.Fail("company", "Company name required for Supplier user", 422, false);

            if (model.Password != model.ConfirmPassword)
                return ApiResponse<int>.Fail("password", "Password doesn't match with confirm password", 422, false);

            var user = _mapper.Map<User>(model);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(model.Password);

            var result = await _userRepository.RegisterUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.Success(result, 200);
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);
            return dto.Count == 0 ? ApiResponse<List<UserDto>>.Fail("User list is empty", 404) : ApiResponse<List<UserDto>>.Success(dto, 200);
        }


        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var userDatas = await _userRepository.GetByIdAsync(userId);
            var userDto = _mapper.Map<UserDto>(userDatas);
            return userDto;
        }



        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordModel resetPasswordRequestDto)
        {
            var user = await _userRepository.GetByEmailCode(resetPasswordRequestDto.ResetPasswordCode);

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

        public async Task<ApiResponse<NoContentDto>> UpdateUserIdentifierAsync(int userId, string refreshToken, DateTime expirationDate, int addOnAccessTokenDate)
        {
            var isSuccessfull = await _userRepository.UpdateUserTokenAsync(userId, refreshToken, expirationDate, addOnAccessTokenDate);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<NoContentDto>.Success(200);
        }

        public async Task<int> GetIdentityNameAsIntAsync(string name)
        {
            return await _userRepository.ConvertIdentity(name);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<ApiResponse<bool>> SendResetPasswordEmail(string email)
        {
            var userExsist = await _userRepository.GetByEmailAsync(email);

            Random random = new Random();
            var stringCode = random.Next(0, 999999).ToString();

            if (userExsist == null)
                return ApiResponse<bool>.Fail($"We can't found this email: {email}", 404);

            await _userRepository.SetEmailCode(stringCode, userExsist.Id);

            await _mailService.SendPasswordResetMailAsync(email, stringCode);

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<UserDto>> GetUserByNameAsync(string name)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
                return ApiResponse<UserDto>.Fail("User not found", 404);

            var dto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(dto, 200);
        }


        public async Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetActiveUsersAsync();
            var dto = _mapper.Map<List<ActiveUserDto>>(users);

            return ApiResponse<List<ActiveUserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersWithoutCurrentUserAsync(string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
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

        public async Task<ApiResponse<List<UserMainDto>>> GetUserWFAAsync(string name, int userStatus, int userType, int page, int limit)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserWFAAsync(userId, userStatus, userType, page, limit);
            var dto = _mapper.Map<List<UserMainDto>>(users.Item2);

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200, users.Item1);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);

        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserAllAsync(string name, int userStatus, int userType, int page, int limit)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserAllAsync(userId, userStatus, userType, page, limit);
            var dto = _mapper.Map<List<UserMainDto>>(users.Item2);



            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200, users.Item1);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserCompanyAsync(string name, int userStatus, int page, int limit)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserCompanyAsync(userId, userStatus, page, limit);
            var dto = _mapper.Map<List<UserMainDto>>(users.Item2);


            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200, users.Item1);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserVendorAsync(string name, int userStatus, int page, int limit)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserVendorAsync(userId, userStatus, page, limit);
            var dto = _mapper.Map<List<UserMainDto>>(users.Item2);

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200, users.Item1);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<bool>> UserChangeStatusAsync(string name, UserChangeStatusModel model)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            if (model.Id == 0)
                return ApiResponse<bool>.Fail("User not found", 404);

            var user = await _userRepository.UserChangeStatusAsync(userId, model);

            await _unitOfWork.SaveChangesAsync();
            if (user)
                return ApiResponse<bool>.Success(true, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<bool>> UserChangeStatusAsync(string name, List<UserChangeStatusModel> model)
        {
            var table = model.ConvertToDataTable();
            var userId = await _userRepository.ConvertIdentity(name);
            var user = await _userRepository.UserChangeStatusAsync(userId, table);
            await _unitOfWork.SaveChangesAsync();
            if (user)
                return ApiResponse<bool>.Success(true, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<UserLoadDto>> GetUserInfoAsync(int userId)
        {
            var user = await _userRepository.GetUserInfoAsync(userId);
            var dto = _mapper.Map<UserLoadDto>(user);

            return ApiResponse<UserLoadDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ERPUserDto>>> GetERPUserAsync()
        {
            var user = await _userRepository.GetERPUser();
            var dto = _mapper.Map<List<ERPUserDto>>(user);
            return ApiResponse<List<ERPUserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<int>> SaveUserAsync(UserSaveModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userEntry = _mapper.Map<User>(user);

            if (!string.IsNullOrEmpty(user.Password))
                userEntry.PasswordHash = SecurityUtil.ComputeSha256Hash(user?.Password);


            var result = await _userRepository.SaveUserAsync(userEntry);
            await _unitOfWork.SaveChangesAsync();

            //if (user?.Photo?.Length > 0)
            //    await _producer.ProduceAsync(user?.Photo, Filetype.Profile, user.Email);

            //if (user?.Signature?.Length > 0)
            //    await _producer.ProduceAsync(user?.Signature, Filetype.Signature, user.Email);


            return result > 0 ? ApiResponse<int>.Success(result, 200)
                          : ApiResponse<int>.Fail("Data can not be saved", 400);
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

        public async Task<ApiResponse<int>> DeleteUserAsync(DeleteUser deleteUser)
        {
            int succesfulCounter = 0;
            List<Task<int>> tasks = new List<Task<int>>();

            deleteUser.userIds.ForEach(x =>
            {
                tasks.Add(_userRepository.SaveUserAsync(new() { Id = x }));
            });

            int[] results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                if (result > 0) succesfulCounter++;
            }

            await _unitOfWork.SaveChangesAsync();

            return succesfulCounter == deleteUser.userIds.Count
                ? ApiResponse<int>.Success(0, 200)
                : ApiResponse<int>.Fail("User can not be deleted", 400);
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

        public async Task<ApiResponse<bool>> AddGroupToUserAsync(List<int> groupsIds, int userId)
        {
            var data = groupsIds.ConvertListToDataTable();
            var result = await _userRepository.AddGroupToUserAsync(data, userId);
            await _unitOfWork.SaveChangesAsync();
            if (result)
                return ApiResponse<bool>.Success(result);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> DeleteGroupFromUserAsync(List<int> groupsIds, int userId)
        {
            var data = groupsIds.ConvertListToDataTable();
            var result = await _userRepository.DeleteGroupFromUserAsync(data, userId);
            await _unitOfWork.SaveChangesAsync();
            if (result)
                return ApiResponse<bool>.Success(result);
            else
                return ApiResponse<bool>.Fail("Data can not be saved", 400);
        }

        public async Task<ApiResponse<bool>> ConfirmEmail(string verifyToken)
        {
            if (string.IsNullOrEmpty(verifyToken))
                return ApiResponse<bool>.Fail("Verify Token is empty", 400);

            var user = await _userRepository.ConfirmEmail(verifyToken);
            await _unitOfWork.SaveChangesAsync();

            if (user)
                return ApiResponse<bool>.Success(true, 200);

            return ApiResponse<bool>.Fail("Email can not be confirmed", 400);
        }

        public async Task<bool> CheckEmailIsVerified(string email)
        {
            var result = await _userRepository.CheckEmailIsVerified(email);
            return result;
        }

        public async Task<UserData> GetUserDataByVerifyTokenAsync(string verifyToken)
        {
            var result = await _userRepository.GetUserDataByVerifyTokenAsync(verifyToken);
            return result;
        }

        public async Task<List<string>> GetAdminUsersAsync(int sequence, Language language)
        {
            var users = await _userRepository.GetAdminUsersAsync(sequence, language);
            return users;
        }

        public async Task<bool> CheckUserVerifyByVendor(string email)
        {
            var result = await _userRepository.CheckUserVerifyByVendor(email);
            return result;
        }
    }
}
