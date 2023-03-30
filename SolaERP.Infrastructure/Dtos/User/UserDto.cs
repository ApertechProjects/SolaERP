using SolaERP.Infrastructure.Attributes;
using SolaERP.Infrastructure.Enums;
using System.Text.Json.Serialization;

namespace SolaERP.Infrastructure.Dtos.UserDto
{
    public class UserDto
    {
        [DbColumn("Id")]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int StatusId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Gender { get; set; }
        public bool VendorId { get; set; }
        [JsonIgnore]
        public Guid UserToken { get; set; }
        public UserRegisterType UserType { get; set; }
    }
}
