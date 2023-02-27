using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.User;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IUserRepository : ICrudOperations<User>
    {
        Task<User> GetUserByUsernameAsync(string userName);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetLastInsertedUserAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<int> GetUserIdByTokenAsync(string finderToken);
        Task<string> GetUserNameByTokenAsync(string finderToken);
        Task<bool> UpdateUserTokenAsync(int userId, Guid token);
        Task<bool> ResetUserPasswordAsync(string email, string passwordHash);
        Task<List<ActiveUser>> GetActiveUsersAsync();
    
    }
}
