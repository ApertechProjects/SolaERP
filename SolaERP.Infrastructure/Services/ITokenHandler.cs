using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.UserDto;
using System.Security.Claims;

namespace SolaERP.Infrastructure.Services
{
    public interface ITokenHandler
    {
        Task<Token> GenerateJwtTokenAsync(int days, UserDto dto);
        Task<List<Claim>> GetUserClaimsAsync(UserDto dto);

    }
}
