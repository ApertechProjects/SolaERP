using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Dtos.UserDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public async Task<Token> GenerateJwtTokenAsync(int minutes, UserDto user)
        {
            Token result = await Task.Run(async () =>
            {
                Token token = new Token();
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
                SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                token.Expiration = DateTime.Now.AddMinutes(minutes);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _configuration["Token:Audience"],
                    Issuer = _configuration["Token:Issuer"],
                    Expires = token.Expiration,
                    NotBefore = DateTime.UtcNow,
                    Subject = new ClaimsIdentity(await GetUserClaimsAsync(user)),
                    SigningCredentials = signingCredentials

                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                token.AccessToken = tokenHandler.WriteToken(securityToken);

                return token;
            });
            return result;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(UserDto user)
        {
            var claims = await Task.Run(() =>
            {
                var value = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                };
                return value;
            });

            return claims;
        }


    }
}
