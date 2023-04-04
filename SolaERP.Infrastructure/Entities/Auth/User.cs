using Newtonsoft.Json;
using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities.Auth
{
    public class User : BaseEntity
    {
        private string theme = "light";
        public int Id { get; set; } = 0;
        public string FullName { get; set; }
        public bool ChangePassword { get; set; }
        public int StatusId { get; set; }
        public string Theme
        {
            get
            {
                return theme;
            }
            set
            {
                theme = value;
            }
        }
        public DateTime LastActivity { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [DbIgnore]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
        public int VendorId { get; set; }
        public Guid UserToken { get; set; }
        public bool IsDeleted { get; set; }
        public bool Gender { get; set; }
        public string Buyer { get; set; }
        public string Description { get; set; }
        public string ERPUser { get; set; }
    }
}
