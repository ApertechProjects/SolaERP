using SolaERP.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Models
{
    public class UserRegisterModel
    {
        public int Id { get; set; }
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
