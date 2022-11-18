using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Repositories
{
    public interface IUserRepository : ICrudOperations<User>
    {
        User GetByUserName(string userName);
        User GetByEmail(string email);
    }
}
