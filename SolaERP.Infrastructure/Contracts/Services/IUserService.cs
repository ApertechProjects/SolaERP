using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<UserDto> GetByUserId(int userId);
        Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<ApiResponse<bool>> UpdateUserPassword(UserUpdatePasswordDto userUpdatePasswordDto);
       // Task<ApiResponse<List<MenuLoadDto>>> GetUserMenusAsync();
    }
}
