using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SolaERP.Application.Services
{
    public class JwtTokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public JwtTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Token> GenerateJwtTokenAsync(int minutes)
        {
            Token token = new Token();
            return await Task.Run(() =>
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
                SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                token.Expiration = DateTime.Now.AddMinutes(minutes);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _configuration["Token:Audience"],
                    Issuer = _configuration["Token:Issuer"],
                    Expires = token.Expiration,
                    NotBefore = DateTime.UtcNow,
                    SigningCredentials = signingCredentials,
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                token.AccessToken = tokenHandler.WriteToken(securityToken);

                return token;
            });
        }

    }
}
