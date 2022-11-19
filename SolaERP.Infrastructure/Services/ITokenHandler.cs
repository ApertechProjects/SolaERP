using SolaERP.Infrastructure.Dtos.Auth;

namespace SolaERP.Infrastructure.Services
{
    public interface ITokenHandler
    {
        Token GenerateJwtTokenAsync();
    }
}
