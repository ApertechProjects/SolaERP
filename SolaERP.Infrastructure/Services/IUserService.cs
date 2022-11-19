using SolaERP.Infrastructure.Dtos;
using SolaERP.Infrastructure.Dtos.Auth;

namespace SolaERP.Infrastructure.Services
{
    public interface IUserService : IBaseService<UserDto>
    {
        Task<ApiResponse<Token>> LoginAsync(LoginRequestDto user);
    }
}
