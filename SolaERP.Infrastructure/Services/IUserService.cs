using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.User;
using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Infrastructure.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<UserDto> GetByUserId(int userId);
        Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto userUpdateDto);
  
    }
}
