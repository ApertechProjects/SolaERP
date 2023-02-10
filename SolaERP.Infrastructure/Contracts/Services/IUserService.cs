using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<ApiResponse<UserDto>> GetUserByTokenAsync(string finderToken);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<ApiResponse<NoContentDto>> UpdateUserIdentifierAsync(string finderToken, Guid newToken);
        Task<ApiResponse<bool>> SendResetPasswordEmail(string email, string templatePath);
        Task<int> GetUserIdByTokenAsync(string finderToken);
        Task<ApiResponse<bool>> RemoveUserByTokenAsync(string finderToken);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<ApiResponse<List<ActiveUserDto>>> GetActiveUsersAsync();
    }
}
