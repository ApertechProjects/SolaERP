using AutoMapper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Constants;
using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Group;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Dtos.UserDto;
using SolaERP.Application.Dtos.UserReport;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Entities.Request;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Entities.UserReport;
using SolaERP.Application.Entities.Vendors;
using SolaERP.Application.Enums;
using SolaERP.Application.Extensions;
using SolaERP.Application.Models;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.ViewModels;
using SolaERP.Persistence.Utils;

namespace SolaERP.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly IAttachmentRepository _attachmentRepo;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IConfiguration _configuration;
        private readonly IApproveStageService _approveStageMainService;
        private readonly IVendorService _vendorService;
        private readonly IApproveStageService _approveStageService;

        public UserService(IUserRepository userRepository,
            IGroupRepository groupRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMailService mailService,
            ITokenHandler tokenHandler,
            IEmailNotificationService emailNotificationService,
            IAttachmentRepository attachmentRepo,
            IFileUploadService fileUploadService,
            IConfiguration configuration,
            IApproveStageService approveStageMainService,
            IVendorService vendorService)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _emailNotificationService = emailNotificationService;
            _attachmentRepo = attachmentRepo;
            _fileUploadService = fileUploadService;
            _configuration = configuration;
            _approveStageMainService = approveStageMainService;
            _vendorService = vendorService;
        }


        public async Task<ApiResponse<int>> UserRegisterAsync(UserRegisterModel model)
        {
            var userExsist = await _userRepository.GetByEmailAsync(model.Email);

            if (userExsist is not null)
                return ApiResponse<int>.Fail("user", "This user is already exist in our system", 422, false);

            if (model.Password != model.ConfirmPassword)
                return ApiResponse<int>.Fail("password", "Password doesn't match with confirm password", 422, false);

            var user = _mapper.Map<User>(model);
            user.PasswordHash = SecurityUtil.ComputeSha256Hash(model.Password);

            var result = await _userRepository.RegisterUserAsync(user);

            await _userRepository.UpdateLastActivityAsync(result);

            if (user.UserTypeId == 0)
            {
                var companyInfo = await _vendorService.GetByTaxAsync(model.TaxId);
                int groupId = 0;
                if (companyInfo.VendorCode == null)
                {
                    groupId = await _groupRepository.GetGroupIdByVendorAdmin();
                    if (groupId != 0)
                        await _userRepository.AddDefaultVendorAccessToVendorUser(groupId, result);
                    await AutoApproveForSupplierUser(result, companyInfo);
                }
                else
                {
                    groupId = await _groupRepository.GetGroupIdByVendorUser();
                    if (groupId != 0)
                        await _userRepository.AddDefaultVendorAccessToVendorUser(groupId, result);
                }


                await UpdateUserStatusAsync(result);
            }

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.Success(result, 200);
        }

        private async Task AutoApproveForSupplierUser(int userId, VendorInfo companyInfo)
        {
            if (companyInfo.VendorCode == null)
            {
                var stageCount = await _approveStageMainService.GetStageCountAsync(Procedures.Users);
                for (int i = 0; i < stageCount; i++)
                {
                    await UserChangeStatusAsync(userId.ToString(), new UserChangeStatusModel
                    {
                        ApproveStatus = 1,
                        Comment = "auto approve",
                        Id = userId,
                        Sequence = i + 1,
                    });
                }
                await UpdateUserStatusAsync(userId);
            }


        }

        private async Task UpdateUserStatusAsync(int userId)
        {
            await _userRepository.UpdateUserStatusAsync(userId);
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var dto = _mapper.Map<List<UserDto>>(users);
            return dto.Count == 0
                ? ApiResponse<List<UserDto>>.Fail("User list is empty", 404)
                : ApiResponse<List<UserDto>>.Success(dto, 200);
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

            if (user is null || string.IsNullOrWhiteSpace(user.Email))
            {
                return ApiResponse<bool>.Fail("Something went wrong, Please contact administrator", 404);
            }
            if (resetPasswordRequestDto.Password == resetPasswordRequestDto.ConfirmPassword)
            {
                user.PasswordHash = SecurityUtil.ComputeSha256Hash(resetPasswordRequestDto.Password);
                await _userRepository.ResetUserPasswordAsync(user.Email, user.PasswordHash);


                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.Success(true, 200);
            }

            return ApiResponse<bool>.Fail("password", "Password does not match with ConfirmPassword", 422);
        }

        public async Task<ApiResponse<NoContentDto>> UpdateUserIdentifierAsync(int userId, string refreshToken,
            DateTime expirationDate, int addOnAccessTokenDate)
        {
            var isSuccessfull =
                await _userRepository.UpdateUserTokenAsync(userId, refreshToken, expirationDate, addOnAccessTokenDate);
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

        public async Task<ApiResponse<string>> SendResetPasswordEmail(string email)
        {
            var userExsist = await _userRepository.GetByEmailAsync(email);

            Random random = new Random();
            var stringCode = random.Next(100000, 999999).ToString();

            if (userExsist == null)
                return ApiResponse<string>.Fail($"We can't found this email: {email}", 404);

            var result = await _userRepository.SetEmailCode(stringCode, userExsist.Id);


            await _unitOfWork.SaveChangesAsync();
            if (result)
                return ApiResponse<string>.Success(stringCode, 200);
            return ApiResponse<string>.Fail("Email code can not be saved", 400);

        }

        public async Task<ApiResponse<UserDto>> GetUserByNameAsync(string name, string token)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
                return ApiResponse<UserDto>.Fail("User not found", 404);

            var dto = _mapper.Map<UserDto>(user);
            SetUserPhoto(dto);
            return ApiResponse<UserDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<UserDto>> GetCurrentUserInfo(string name)
        {
            var userId = await _userRepository.ConvertIdentity(name);
            var user = await _userRepository.GetCurrentUserInfo(userId);

            if (user is null)
                return ApiResponse<UserDto>.Fail("User not found", 404);

            var dto = _mapper.Map<UserDto>(user);
            SetUserPhoto(dto);
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

        public async Task<ApiResponse<List<UserMainDto>>> GetUserWFAAsync(string name, int userStatus, int userType)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserWFAAsync(userId, userStatus, userType);

            var dto = _mapper.Map<List<UserMainDto>>(users);
            SetUserPhotoMany(dto);

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserAllAsync(string name, int userStatus, int userType)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserAllAsync(userId, userStatus, userType);
            var dto = _mapper.Map<List<UserMainDto>>(users);
            SetUserPhotoMany(dto);
            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserCompanyAsync(string name, int userStatus)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserCompanyAsync(userId, userStatus);
            var dto = _mapper.Map<List<UserMainDto>>(users);
            SetUserPhotoMany(dto);

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
            return ApiResponse<List<UserMainDto>>.Fail("User list is empty", 404);
        }

        public async Task<ApiResponse<List<UserMainDto>>> GetUserVendorAsync(string name, int userStatus)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var users = await _userRepository.GetUserVendorAsync(userId, userStatus);
            var dto = _mapper.Map<List<UserMainDto>>(users);
            SetUserPhotoMany(dto);

            if (dto.Count > 0)
                return ApiResponse<List<UserMainDto>>.Success(dto, 200);
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
            var table = model.ConvertListOfCLassToDataTable();
            var userId = await _userRepository.ConvertIdentity(name);
            var user = await _userRepository.UserChangeStatusAsync(userId, table);
            await _unitOfWork.SaveChangesAsync();


            if (user)
                return ApiResponse<bool>.Success(true, 200);
            return ApiResponse<bool>.Fail("Problem detected", 400);
        }

        public async Task<ApiResponse<UserLoadDto>> GetUserInfoAsync(int userId, string token)
        {
            var user = await _userRepository.GetUserInfoAsync(userId);
            user.SignaturePhoto = _fileUploadService.GetFileLink(user.SignaturePhoto, Modules.Users);
            user.UserPhoto = _fileUploadService.GetFileLink(user.UserPhoto, Modules.Users);
            var attachments = await _attachmentRepo.GetAttachmentsAsync(user.Id, null, "PYMDC");

            var dto = _mapper.Map<UserLoadDto>(user);
            if (userId == 0)
            {
                dto.Gender = null;
                dto.VendorId = null;
            }

            return ApiResponse<UserLoadDto>.Success(dto, 200);
        }

        public async Task<ApiResponse<List<ERPUserDto>>> GetERPUserAsync()
        {
            var user = await _userRepository.GetERPUser();
            var dto = _mapper.Map<List<ERPUserDto>>(user);
            return ApiResponse<List<ERPUserDto>>.Success(dto, 200);
        }

        public async Task<ApiResponse<int>> SaveUserAsync(UserSaveModel user,
            CancellationToken cancellationToken)
        {
            if (user.Id == 0)
            {
                var userInDb = await _userRepository.GetByEmailAsync(user.Email);
                if (userInDb is not null)
                    ApiResponse<int>.Fail("This mail is already in use", 400);
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (user.Description == "null")
            {
                user.Description = null;
            }

            if (user.ERPUser == "null")
            {
                user.ERPUser = null;
            }

            var userEntry = _mapper.Map<User>(user);


            if (!string.IsNullOrEmpty(user.Password))
                userEntry.PasswordHash = SecurityUtil.ComputeSha256Hash(user?.Password);

            UserImage userImage = await _userRepository.UserImageData(user.Id);
            userEntry.UserPhoto =
                await _fileUploadService.GetLinkForEntity(user.UserPhoto, Modules.Users, user.UserPhotoIsDeleted,
                    userImage.UserPhoto);
            userEntry.SignaturePhoto = await _fileUploadService.GetLinkForEntity(user.SignaturePhoto, Modules.Users,
                user.SignaturePhotoIsDeleted,
                userImage.SignaturePhoto);

            var result = await _userRepository.SaveUserAsync(userEntry);

            await _unitOfWork.SaveChangesAsync();
            return result > 0
                ? ApiResponse<int>.Success(result, 200)
                : ApiResponse<int>.Fail("Data can not be saved", 400);
        }


        public async Task<ApiResponse<bool>> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel)
        {
            if (passwordModel.Password != passwordModel.ConfirmPassword)
                return ApiResponse<bool>.Fail("password", "Password must be equal to Confirm password", 422);
            passwordModel.Password = SecurityUtil.ComputeSha256Hash(passwordModel.Password);
            var pass = await _userRepository.ChangeUserPasswordAsync(passwordModel);

            await _unitOfWork.SaveChangesAsync();

            if (pass) return ApiResponse<bool>.Success(pass, 200);

            else return ApiResponse<bool>.Fail(ResultMessageConstants.OperationUnsucces, 400);
        }

        public async Task<ApiResponse<int>> DeleteUserAsync(DeleteUser deleteUser)
        {
            int succesfulCounter = 0;
            List<Task<int>> tasks = new List<Task<int>>();

            deleteUser.userIds.ForEach(x => { tasks.Add(_userRepository.SaveUserAsync(new() { Id = x })); });

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

            return dto.Capacity > 0
                ? ApiResponse<List<UsersByGroupDto>>.Success(dto, 200)
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
            {
                //var userType = await CheckUserType(verifyToken);
                //if (userType == "0")
                //	return ApiResponse<bool>.Success(true, 200);
                //else
                //{
                UserData userData = await GetUserDataByVerifyTokenAsync(verifyToken);
                var approvalCount = await UserApprovalCount(userData.Id);

                #region RegistratedUser

                #endregion

                #region AdminUsers
                if (approvalCount > 0)
                {
                    await _mailService.SendRegistrationPendingMail(userData.Id);
                    //await _mailService.SendMailToAdminstrationForApproveRegistration(userData.Id);
                }
                else
                {
                    //await _mailService.SendMailToAdminstrationAboutRegistration(userData.Id);
                }
                #endregion
                //}
            }

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

        public async Task<List<string>> GetAdminUserMailsAsync(int sequence, Language language)
        {
            var users = await _userRepository.GetAdminUserMailsAsync(sequence, language);
            return users;
        }

        public async Task<bool> CheckUserVerifyByVendor(string email)
        {
            var result = await _userRepository.CheckUserVerifyByVendor(email);
            return result;
        }

        public async Task<string> CheckUserType(string verifyToken)
        {
            var result = await _userRepository.CheckUserType(verifyToken);
            return result;
        }

        private void SetUserPhotoMany(List<UserMainDto> userDtoList)
        {
            userDtoList.ForEach(SetUserPhoto);
        }

        private void SetUserPhoto(UserMainDto userMainDto)
        {
            userMainDto.UserPhoto = _fileUploadService.GetFileLink(userMainDto.UserPhoto, Modules.Users);
        }

        private void SetUserPhoto(UserDto userDto)
        {
            userDto.UserPhoto = _fileUploadService.GetFileLink(userDto.UserPhoto, Modules.Users);
        }

        public async Task<List<Application.Dtos.User.UserList>> UsersRequestDetails(int? requestDetailId, int? sequence,
            ApproveStatus status)
        {
            var users = await _userRepository.UsersRequestDetails(requestDetailId, sequence, status);
            var dto = _mapper.Map<List<Application.Dtos.User.UserList>>(users);
            return dto;
        }

        public async Task<List<Application.Dtos.User.UserList>> UsersForRequestMain(int requestMainId, int? sequence,
            ApproveStatus status)
        {
            var users = await _userRepository.UsersRequestMain(requestMainId, sequence, status);
            var dto = _mapper.Map<List<Application.Dtos.User.UserList>>(users);
            return dto;
        }

        public async Task UpdateUserLastActivity(int id)
        {
            await _userRepository.UpdateLastActivityAsync(id);
        }

        public async Task<ApiResponse<bool>> CheckVerifyCode(int verificationCode)
        {
            var user = await _userRepository.GetByEmailCode(verificationCode);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
                return ApiResponse<bool>.Fail("resetPasswordCode", $"You entered wrong Code", 422);
            return ApiResponse<bool>.Success(true, 200);
        }

        public async Task<ApiResponse<List<UserReportHasAccessDto>>> GetUserReportAccess(string fileId)
        {
            var data = await _userRepository.GetUserReportAccess(fileId);
            return ApiResponse<List<UserReportHasAccessDto>>.Success(data, 200);
        }

        public async Task<ApiResponse<List<UserReportFileAccess>>> GetUserReportAccessByCurrentUser(string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var data = await _userRepository.GetUserReportAccessByCurrentUser(userId);
            var map = _mapper.Map<List<UserReportFileAccess>>(data);
            return ApiResponse<List<UserReportFileAccess>>.Success(map, 200);
        }

        public async Task<ApiResponse<bool>> ChangeUserLanguage(string name, string language)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var changeLang = await _userRepository.ChangeUserLanguage(userId, language);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.Success(changeLang, 200);
        }

        private static VM_RegistrationIsPendingAdminApprove GetVM(string companyName, User user,
         EmailTemplateData templateData)
        {
            return new()
            {
                Body = new HtmlString(templateData.Body),
                CompanyName = "",
                Header = templateData.Header,
                UserName = user.UserName,
                CompanyOrVendorName = "",
                Language = templateData.Language.GetLanguageEnumValue(),
            };
        }

        public async Task<ApiResponse<bool>> UserSendToApprove(string name)
        {
            int userId = await _userRepository.ConvertIdentity(name);
            var res = await _userRepository.UserSendToApprove(userId);

            int groupId = await _groupRepository.GetGroupIdByVendorAdmin();
            if (groupId != 0)
            {
                int groupIdVendorUser = await _groupRepository.GetGroupIdByVendorUser();
                await _userRepository.AddDefaultVendorAccessToVendorUser(groupIdVendorUser, userId);

                int groupuserIdByVendorAdmin = await _userRepository.GetVendorAdminGroupUserByUserId(userId);
                await _userRepository.RemoveUserFromVendorAdmin(groupuserIdByVendorAdmin);
                await _unitOfWork.SaveChangesAsync();
            }

            await _unitOfWork.SaveChangesAsync();

            await _mailService.SendRegistrationPendingMail(userId);
            //await _mailService.SendMailToAdminstrationForApproveRegistration(userId);

            return ApiResponse<bool>.Success(res, 200);
        }

        public async Task<int> UserApprovalCount(int userId)
        {
            var data = await _userRepository.UserApprovalCount(userId);
            return data;
        }
    }
}