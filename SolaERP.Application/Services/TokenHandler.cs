using Microsoft.AspNetCore.Http;
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
		private readonly IHttpContextAccessor _httpContextAccessor;

		public JwtTokenHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}


		public string CreateRefreshToken()
		{
			byte[] number = new byte[32];
			using RandomNumberGenerator random = RandomNumberGenerator.Create();
			random.GetBytes(number);
			return Convert.ToBase64String(number);

		}

		//private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		//{
		//	var jwtSettings = _configuration.GetSection("JwtSettings");
		//	var tokenValidationParameters = new TokenValidationParameters
		//	{
		//		ValidateAudience = true,
		//		ValidateIssuer = true,
		//		ValidateIssuerSigningKey = true,
		//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
		//		ValidateLifetime = true,
		//		ValidIssuer = jwtSettings["validIssuer"],
		//		ValidAudience = jwtSettings["validAudience"]
		//	};

		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	SecurityToken securityToken;
		//	var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

		//	var jwtSecurityToken = securityToken as JwtSecurityToken;

		//	if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
		//	{

		//	}
		//}


		public async Task<Token> GenerateJwtTokenAsync(int hour, UserRegisterModel user)
		{
			Token result = await Task.Run(async () =>
			{
				Token token = new Token();
				SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
				SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

				token.Expiration = DateTime.Now.AddHours(hour);
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

		public string GetAccessToken()
		{
			if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
			  authHeader.ToString().StartsWith("Bearer "))
			{
				var accessToken = authHeader.ToString().Substring("Bearer ".Length);
				return accessToken;
			}
			return null;
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
