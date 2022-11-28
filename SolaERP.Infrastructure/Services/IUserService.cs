using SolaERP.Infrastructure.Dtos.UserDto;

namespace SolaERP.Infrastructure.Services
{
    public interface IUserService : ICrudService<UserDto>
    {
        Task<UserDto> GetByUserId(int userId);
    }
}
