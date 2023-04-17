using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Groups;
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
        Task<User> GetUserByEmailCode(string token);
        Task<bool> SetUserEmailCode(string token, int id);
        Task<int> GetIdentityNameAsIntAsync(string name);
        Task<string> GetUserNameByTokenAsync(string finderToken);
        Task<bool> UpdateUserTokenAsync(int userId, Guid token);
        Task<bool> ResetUserPasswordAsync(string email, string passwordHash);
        Task<List<ActiveUser>> GetActiveUsersAsync();
        Task<List<ActiveUser>> GetActiveUsersWithoutCurrentUserAsync(int userId);
        Task<List<UserMain>> GetUserWFAAsync(int userId, int userStatus, int userType);
        Task<List<UserMain>> GetUserAllAsync(int userId, int userStatus, int userType);
        Task<List<UserMain>> GetUserCompanyAsync(int userId, int userStatus);
        Task<List<UserMain>> GetUserVendorAsync(int userId, int userStatus);
        Task<bool> UserChangeStatusAsync(int userId, UserChangeStatusModel model);
        Task<bool> SaveUserAsync(User model);
        Task<UserLoad> GetUserInfoAsync(int userId);
        Task<List<ERPUser>> GetERPUser();
        Task<bool> CheckTokenAsync(Guid authToken);
        Task<bool> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel);
        Task<List<UsersByGroup>> GetUsersByGroupIdAsync(int groupId);
    }
}
