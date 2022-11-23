namespace SolaERP.Infrastructure.Dtos.Auth
{
    public class AccountResponseDto
    {
        public Token Token { get; set; }
        public UserDto.UserDto AccountUser { get; set; }
    }
}
