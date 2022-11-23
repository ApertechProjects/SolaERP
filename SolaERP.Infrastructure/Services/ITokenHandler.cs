using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Entities.Auth;
using System.Security.Claims;

namespace SolaERP.Infrastructure.Services
{
    public interface ITokenHandler
    {
        Task<Token> GenerateJwtTokenAsync(User dto, int days);
        IEnumerable<Claim> GetClaims(User dto);
    }
}
