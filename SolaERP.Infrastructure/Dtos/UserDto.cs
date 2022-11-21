using SolaERP.Infrastructure.Enums;

namespace SolaERP.Infrastructure.Dtos
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPasswordHash { get; set; }
        public UserRegisterType UserTypeId { get; set; }
    }
}
