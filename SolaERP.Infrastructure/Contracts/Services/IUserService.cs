namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<UserDto> GetByUserId(int userId);
        Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<ApiResponse<bool>> UpdateUserPassword(UserUpdatePasswordDto userUpdatePasswordDto);
        Task<ApiResponse<NoContentDto>> UpdateUserIdentifier(string finderToken, Guid newToken);
        Task<int> GetUserIdByTokenAsync(string finderToken);
    }
}
