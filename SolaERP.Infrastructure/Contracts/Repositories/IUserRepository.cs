using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.User;
using SolaERP.Infrastructure.Models;

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
        Task<List<UserMain>> GetUserWFAAsync(int userId, UserGetModel model);
        Task<List<UserMain>> GetUserAllAsync(int userId, UserGetModel model);
        Task<List<UserMain>> GetUserCompanyAsync(int userId, List<int> userStatus, bool all);
        Task<List<UserMain>> GetUserVendorAsync(int userId, List<int> userStatus, bool all);
    }
}
