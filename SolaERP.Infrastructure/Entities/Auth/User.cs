using SolaERP.Infrastructure.Attributes;

namespace SolaERP.Infrastructure.Entities.Auth
{
    public class User : BaseEntity
    {
        [DbColumn("Id")]
        public int UserId { get; set; }
        private string theme = "light";
        public int Id { get; set; }
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
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
        public int VendorId { get; set; }
        public Guid UserToken { get; set; }
        public bool IsDeleted { get; set; }
        public int Gender { get; set; }
        public string Buyer { get; set; }
        public string Description { get; set; }
        public string ERPUser { get; set; }
    }
}
