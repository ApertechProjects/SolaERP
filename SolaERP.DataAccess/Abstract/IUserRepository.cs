using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.DataAccess.Abstract
{
    public interface IUserRepository : ICrudOperations<User>
    {
        User GetByUserName(string userName);
        User GetByEmail(string email);
    }
}
