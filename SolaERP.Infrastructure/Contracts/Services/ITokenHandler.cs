using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Models;
using System.Security.Claims;

namespace SolaERP.Application.Contracts.Services
{
    public interface ITokenHandler
    {
        Task<Token> GenerateJwtTokenAsync(int minutes, UserRegisterModel dto);
        Task<List<Claim>> GetUserClaimsAsync(UserRegisterModel dto);
        string CreateRefreshToken();

    }
}
