using SolaERP.Application.Entities;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.Groups;
using SolaERP.Application.Entities.User;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using System.Data;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByUsernameAsync(string userName);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByEmailCode(string token);
        Task<bool> SetEmailCode(string token, int id);
        Task<int> ConvertIdentity(string name);
        Task<string> GetUserNameByTokenAsync(string finderToken);
        Task<bool> UpdateUserTokenAsync(int userId, string refreshtoken, DateTime expirationDate, int refreshTokenLifeTime);
        Task<bool> ResetUserPasswordAsync(string email, string passwordHash);
        Task<List<ActiveUser>> GetActiveUsersAsync();
        Task<List<ActiveUser>> GetActiveUsersWithoutCurrentUserAsync(int userId);
        Task<(int, List<UserMain>)> GetUserWFAAsync(int userId, int userStatus, int userType, int page, int limit);
        Task<(int, List<UserMain>)> GetUserAllAsync(int userId, int userStatus, int userType, int page, int limit);
        Task<(int, List<UserMain>)> GetUserCompanyAsync(int userId, int userStatus, int page, int limit);
        Task<(int, List<UserMain>)> GetUserVendorAsync(int userId, int userStatus, int page, int limit);
        Task<bool> UserChangeStatusAsync(int userId, UserChangeStatusModel model);
        Task<bool> UserChangeStatusAsync(int userId, DataTable data);
        Task<int> SaveUserAsync(User model);
        Task<int> RegisterUserAsync(User model);
        Task<UserLoad> GetUserInfoAsync(int userId);
        Task<List<ERPUser>> GetERPUser();
        Task<bool> ChangeUserPasswordAsync(ChangeUserPasswordModel passwordModel);
        Task<List<UsersByGroup>> GetUsersByGroupIdAsync(int groupId);
        Task<bool> UpdateSessionAsync(int userId, int command); // -1 is Dissconnect 1 Connect
        Task<bool> AddGroupToUserAsync(DataTable data, int userId);
        Task<bool> DeleteGroupFromUserAsync(DataTable data, int userId);
        Task<bool> UpdateImgesAsync(string email, Filetype type, string filePath);
        Task<bool> ConfirmEmail(string verifyToken);
        Task<bool> CheckEmailIsVerified(string email);
        Task<UserData> GetUserDataByVerifyTokenAsync(string verifyToken);
        Task<List<string>> GetAdminUsersAsync(int sequence, Language language);
        Task<bool> CheckUserVerifyByVendor(string email);
    }

    public enum Filetype { Profile = 1, Signature = 2 }
}