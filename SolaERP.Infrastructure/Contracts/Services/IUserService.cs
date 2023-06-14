using SolaERP.Application.Dtos.Group;
using SolaERP.Application.Dtos.Shared;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Dtos.UserDto;
using SolaERP.Application.Entities;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserListDto>>> GetUserListAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<ApiResponse<NoContentDto>> UpdateUserIdentifierAsync(int userId, string refreshToken, DateTime expirationDate, int addOnAccessTokenDate);
        Task<ApiResponse<bool>> SendResetPasswordEmail(string email);
        Task<int> GetIdentityNameAsIntAsync(string name);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordModel resetPasswordRequestDto);
        Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersAsync();
        Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersWithoutCurrentUserAsync(string name);
        Task<ApiResponse<List<UserMainDto>>> GetUserWFAAsync(string name, int userStatus, int userType, int page, int limit);
        Task<ApiResponse<List<UserMainDto>>> GetUserAllAsync(string name, int userStatus, int userType, int page, int limit);
        Task<ApiResponse<List<UserMainDto>>> GetUserCompanyAsync(string name, int userStatus, int page, int limit);
        Task<ApiResponse<List<UserMainDto>>> GetUserVendorAsync(string name, int userStatus, int page, int limit);
        Task<ApiResponse<bool>> UserChangeStatusAsync(string name, UserChangeStatusModel model);
        Task<ApiResponse<bool>> UserChangeStatusAsync(string name, List<UserChangeStatusModel> model);
        Task<ApiResponse<int>> SaveUserAsync(UserSaveModel user, CancellationToken cancellationToken);
        Task<ApiResponse<int>> UserRegisterAsync(UserRegisterModel model);
        Task<ApiResponse<UserLoadDto>> GetUserInfoAsync(int userId);
        Task<ApiResponse<List<ERPUserDto>>> GetERPUserAsync();
        Task<ApiResponse<bool>> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel);
        Task<ApiResponse<int>> DeleteUserAsync(DeleteUser deleteUser);
        Task<bool> UpdateSessionAsync(int userId, int updateCommand);
        Task<ApiResponse<List<UsersByGroupDto>>> GetUsersByGroupIdAsync(int groupId);
        Task<ApiResponse<bool>> AddGroupToUserAsync(List<int> groupsIds, int userId);
        Task<ApiResponse<bool>> DeleteGroupFromUserAsync(List<int> groupsIds, int userId);
        Task<ApiResponse<UserDto>> GetUserByNameAsync(string name);
        Task<ApiResponse<bool>> ConfirmEmail(string verifyToken);
        Task<bool> CheckEmailIsVerified(string email);
        Task<UserData> GetUserDataByVerifyTokenAsync(string verifyToken);
        Task<List<string>> GetAdminUsersAsync(int sequence, Language language);

        Task<bool> CheckUserVerifyByVendor(string email);
    }
}