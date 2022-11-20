using Microsoft.AspNetCore.Identity;
using SolaERP.Application.Utils;
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
            var providedPasswordHash = SecurityUtil.ComputeSha256Hash(providedPassword);

            if (hashedPassword == providedPasswordHash)
                return PasswordVerificationResult.Success;

            return PasswordVerificationResult.Failed;
        }

        #endregion
    }
}
