using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Repositories
{
    public interface IUserRepository : ICrudOperations<User>
    {
        Task<User> GetByUserNameAsync(string userName);
        User GetByEmail(string email);
        Task<User> GetLastInsertedUserAsync();
    }
}
