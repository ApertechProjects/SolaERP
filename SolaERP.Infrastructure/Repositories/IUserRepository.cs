using SolaERP.Infrastructure.Dtos.UserDto;
using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Repositories
{
    public interface IUserRepository : ICrudOperations<User>
    {
        Task<User> GetByUserNameAsync(string userName);
        User GetByEmail(string email);
        Task<User> GetLastInsertedUserAsync();
        Task<User> GetByUserId(int userId);
    }
}
