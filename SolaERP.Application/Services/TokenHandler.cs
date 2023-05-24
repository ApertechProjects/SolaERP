using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.Auth;
using SolaERP.Application.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace SolaERP.Persistence.Services
{
    public class JwtTokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public JwtTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);

        }

        public async Task<Token> GenerateJwtTokenAsync(int days, UserRegisterModel user)
        {
            Token result = await Task.Run(async () =>
            {
                Token token = new Token();
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
                SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                token.Expiration = DateTime.Now.AddDays(days);
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
                token.RefreshToken = CreateRefreshToken();
                return token;
            });
            return result;
        }

        public async Task<List<Claim>> GetUserClaimsAsync(UserRegisterModel user)
        {
            var claims = await Task.Run(() =>
            {
                var value = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.FullName.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString())
                };
                return value;
            });

            return claims;
        }


    }
}
