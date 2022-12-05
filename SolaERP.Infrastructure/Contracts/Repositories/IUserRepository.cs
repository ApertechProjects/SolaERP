using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IUserRepository : ICrudOperations<User>
    {
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetLastInsertedUserAsync();
        Task<User> GetByUserId(int userId);

    }
}
