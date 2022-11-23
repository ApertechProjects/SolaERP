using SolaERP.Infrastructure.Dtos.Auth;
using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Dtos.UserDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPasswordHash { get; set; }
        public UserRegisterType UserTypeId { get; set; }
    }
}
