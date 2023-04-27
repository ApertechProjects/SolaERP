using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Models;
using System.Security.Claims;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface ITokenHandler
    {
        Task<Token> GenerateJwtTokenAsync(int minutes, UserRegisterModel dto);
        Task<List<Claim>> GetUserClaimsAsync(UserRegisterModel dto);
        void DecodeJwtToken(string jwtToken);

    }
}
