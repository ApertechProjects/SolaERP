using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Entities.Auth;
using System.Security.Claims;

namespace SolaERP.Infrastructure.Services
{
    public interface ITokenHandler
    {
        Task<Token> GenerateJwtTokenAsync(int days, User user);
        Task<List<Claim>> GetUserClaimsAsync(User user);

    }
}
