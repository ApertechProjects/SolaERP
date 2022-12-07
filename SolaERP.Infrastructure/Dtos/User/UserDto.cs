using SolaERP.Infrastructure.Enums;
using System.Text.Json.Serialization;

namespace SolaERP.Infrastructure.Dtos.UserDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Photo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [JsonIgnore]
        public Guid UserToken { get; set; }
        public UserRegisterType UserTypeId { get; set; }
    }
}
