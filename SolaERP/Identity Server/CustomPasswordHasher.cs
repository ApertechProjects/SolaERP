
using Microsoft.AspNetCore.Identity;
using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Application.Identity_Server
{
    public class CustomPasswordHasher : IPasswordHasher<User>
    {

        #region PasswordHasher Implementation
        public string HashPassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
