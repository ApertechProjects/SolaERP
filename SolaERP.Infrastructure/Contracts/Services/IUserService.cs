using SolaERP.Infrastructure.Dtos.Group;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Models;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<ApiResponse<List<UserListDto>>> GetUserListAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<ApiResponse<UserDto>> GetUserByTokenAsync(string finderToken);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<ApiResponse<NoContentDto>> UpdateUserIdentifierAsync(string userName, Guid newToken);
        Task<ApiResponse<bool>> SendResetPasswordEmail(string email);
        Task<int> GetUserIdByTokenAsync(string finderToken);
        Task<string> GetUserNameByTokenAsync(string finderToken);
        Task<ApiResponse<bool>> RemoveUserByTokenAsync(string finderToken);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordModel resetPasswordRequestDto);
        Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersAsync();
        Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersWithoutCurrentUserAsync(string finderToken);
        Task<ApiResponse<List<UserMainDto>>> GetUserWFAAsync(string authToken, UserGetModel model);
        Task<ApiResponse<List<UserMainDto>>> GetUserAllAsync(string authToken, UserGetModel model);
        Task<ApiResponse<List<UserMainDto>>> GetUserCompanyAsync(string authToken, int userStatus);
        Task<ApiResponse<List<UserMainDto>>> GetUserVendorAsync(string authToken, int userStatus);
        Task<ApiResponse<bool>> UserChangeStatusAsync(string authToken, UserChangeStatusModel model);
        Task<ApiResponse<bool>> SaveUserAsync(UserSaveModel user);
        Task UserRegisterAsync(UserRegisterModel model);
        Task<ApiResponse<UserLoadDto>> GetUserInfo(int userId);
        Task<ApiResponse<List<ERPUserDto>>> GetERPUserAsync();
        Task<bool> CheckTokenAsync(string authToken);
        Task<ApiResponse<bool>> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel);
        Task<ApiResponse<bool>> DeleteUserAsync(List<int> userIds);
        Task<ApiResponse<List<UsersByGroupDto>>> GetUsersByGroupIdAsync(int groupId);
    }
}
